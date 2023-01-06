using System;
using System.Linq;
using Events.GameEngine.Keyboard;
using GameEngine.GameObjects.Core;
using GameEngine.Resources;
using TMPro;
using UECS;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;
using Utils;
using Utils.Events;
using Rect = Utils.Rect;

namespace GameEngine.UI.Systems
{
    public class InputFieldSystem : ISyncSystem
    {
        private InputFieldComponent _activeInput;
        [AutoInject] private Filter<InputFieldComponent> _filter;
        [ChangeHandler] private Filter<InputFieldComponent> _changed;
        
        public void Init()
        {
            this.RegisterListener();
        }

        public void Update()
        {
            foreach (var result in _changed)
            {
                var component = result.Get1();
                var text = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<TextMeshProUGUI>();
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
                var transform = component.TargetView.Transform;
                if (transform.Childs.FirstOrDefault(c => c.TargetView is UIView)?.TargetView is not { } view)
                {
                    view = component.TargetView.Viewer.ActiveScene.OpenView(new ViewParameters
                    {
                        Parent = transform
                    });
                    view.AddComponent<ImageComponent>().Shader =
                        Undefined.ServerManager.InternalResourcesManager.GetInternalShader(InternalShaderType.Blink);
                }

                var currentLine = component.Text.Split('\n')[^1];
                var y = (int)text.preferredHeight;
                if (string.Empty == currentLine) y = (int)text.GetPreferredValues(component.Text + "!").y;
                if (y != 0) y -= component.CaretSize.Y;
                view.Transform.OriginalRect = new Rect((int)text.GetPreferredValues(currentLine).x, y, component.CaretSize);
                transform.Update();
            }
        }

        [EventHandler]
        private void OnWrite(KeyboardWriteEvent e)
        {
            foreach (var result in _filter)
            {
                foreach (var ch in e.Input)
                {
                    var component = result.Get1();
                    switch (ch)
                    {
                        case '\b':
                            if(component.Text.Length == 0) break;
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
            }
        }
    }
}