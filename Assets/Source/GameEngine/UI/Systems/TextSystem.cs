using GameEngine.GameObjects.Core;
using TMPro;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;

namespace GameEngine.UI.Systems
{
    public class TextSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<Text>> _filter;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _filter)
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
                    text.text = component.Content;
                    component.TargetObject.Transform.Update();
                });
            }
        }
    }
}