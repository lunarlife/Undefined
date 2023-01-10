using System.Linq;
using Events.GameEngine.Keyboard;
using GameEngine.GameObjects.Core;
using GameEngine.Resources;
using TMPro;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Views;
using Utils.Events;
using Rect = Utils.Rect;

namespace GameEngine.UI.Systems
{
    public class InputFieldSystem : ISyncSystem
    {
        private IComponent<InputField> _activeInput;
        private int _cursorOffset = 0;
        [AutoInject] private Filter<IComponent<InputField>> _filter;
        [ChangeHandler] private Filter<IComponent<InputField>> _changed;
        
        public void Init()
        {
            Undefined.OnKeyboardWriting.AddListener(OnWrite);
        }

        public void Update()
        {
            if (Undefined.IsPressed(MouseKey.Left, ClickState.Down))
            {
                _activeInput = Undefined.RaycastUIFirst(Undefined.MouseScreenPosition, out var view) ? view.GetComponent<InputField>() : null;
            }
            foreach (var result in _changed)
            {
                result.Get1().Read(component =>
                {
                    var text = ((ObjectCore)component.TargetObject).GetOrAddUnityComponent<TextMeshProUGUI>();
                    text.color = component.Color.ToUnityColor();
                    text.fontStyle = component.FontStyle.ToUnityStyle();
                    var wrapping = component.Wrapping;
                    text.overflowMode = wrapping.Overflow.ToUnityOverflow();
                    var (vertical, horizontal) = wrapping.Alignment.ToUnityTextAlignment();
                    text.horizontalAlignment = horizontal;
                    text.verticalAlignment = vertical;
                    text.enableWordWrapping = wrapping.IsWrapping;
                    var size = component.Size;
                    text.enableAutoSizing = size.AutoSize;
                    text.fontSize = size.MaxSize;
                    text.fontSizeMax = size.MaxSize;
                    text.fontSizeMin = size.MinSize;
                    text.lineSpacing = size.LineSpacing;
                    text.wordSpacing = size.WordSpacing;
                    text.characterSpacing = size.CharacterSpacing;
                    text.characterWidthAdjustment = size.CharacterWidthAdjustment;
                    text.text = component.Text;
                    IUIView view = null;
                    component.TargetObject.Transform.Read(transform =>
                    {
                        view = (IUIView)transform.Childs.FirstOrDefault(c => c.TargetObject is UIView)?.TargetObject;
                        if (view is not null) return;
                        view =  Undefined.Player.ActiveScene.OpenView(new ViewParameters
                        {
                            Parent = component.TargetObject.Transform
                        });
                        view.AddComponent<ImageComponent>().Modify(image =>
                        {
                            image.Shader = Undefined.ServerManager.InternalResourcesManager
                                .GetInternalShader(InternalShaderType.Blink);
                        });
                    });
                    

                    var currentLine = component.Text.Split('\n')[^1];
                    var y = (int)text.preferredHeight;
                    if (string.Empty == currentLine) y = (int)text.GetPreferredValues(component.Text + "!").y;
                    if (y != 0) y -= component.CaretSize.Y;
                    view.Transform.Modify(transform =>
                    {
                        transform.OriginalRect = new Rect((int)text.GetPreferredValues(currentLine).x, y, component.CaretSize);
                    });
                });
            }
        }

        private void OnWrite(KeyboardWriteEvent e)
        {
            _activeInput?.Read(data =>
            {
                if (e.Input == "\b" && data.Text is "" or "") return;
                _activeInput.Modify(component =>
                {
                    foreach (var ch in e.Input)
                    {
                        switch (ch)
                        {
                            case '\b':
                                if (component.Text.Length == 0) break;
                                component.Text = component.Text[..^1];
                                break;
                            case '\r':
                                component.Text += '\n';
                                break;
                            case '':
                                var split = component.Text.Split('\n');
                                var text = string.Join("\n", split, 0, split.Length - 1);
                                if (split.Length != 1 && split[^1].Length > 1) text += '\n';
                                component.Text = text;
                                break;
                            default:
                                component.Text += ch;
                                break;
                        }
                    }
                });
            });
        }
    }
}