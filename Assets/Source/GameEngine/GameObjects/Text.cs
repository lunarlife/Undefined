using GameEngine.GameObjects.Core;
using TMPro;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Components;
using UndefinedNetworking.GameEngine.UI.Components.Mouse;
using UndefinedNetworking.GameEngine.UI.Elements.Enums;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using Utils;

namespace GameEngine.GameObjects
{
    public class Text : UIElement
    {
        private TMP_Text _textComponent;
        private string _content;
        private readonly int _layer;
        private readonly Side _pivot;
        private readonly Margins _margins;
        private readonly UIElement _parent;
        private readonly UIBind _bind;
        private readonly Rect _rect;
        private readonly FontSize _size;
        private readonly TextWrapping _wrapping;
        private readonly FontStyle _fontStyle;
        private readonly Color _color;


        public Text(Rect rect, FontSize size, Color? color = null, TextWrapping? wrapping = null, string content = "", string name = "", UIBind? bind = null, int layer = 0, Side pivot = Side.Center, Margins? margins = null, UIElement parent = null) 
            : base(parent)
        {
            _color = color ?? Color.Black;
            _rect = rect;
            _size = size;
            _content = content;
            _layer = layer;
            _pivot = pivot;
            _margins = margins ?? new Margins();
            _parent = parent;
            _bind = bind ?? new UIBind
            {
                Side = Side.TopLeft
            };
            _wrapping = wrapping ?? new TextWrapping
            {
                Alignment = TextAlignment.TopLeft,
                Overflow = TextOverflow.Overflow,
                IsWrapping = true
            };
            /*AddUnityComponent((TextMeshProUGUI tmp) =>
            {
                _textComponent = tmp;
                tmp.raycastTarget = false;
                UpdateSize();
                UpdateText();
                UpdateWrapping();
                UpdateColor();
            });*/
        }
        public override ViewParameters CreateNewView(IUIViewer viewer) => new()
        {
            OriginalRect = _rect,
            Bind = _bind,
            Layer = _layer,
            Margins = _margins,
            Pivot = _pivot,
            IsActive = true,
        };

        public override void OnCreateView(IUIView view)
        {
            var component = view.AddComponent<TextComponent>()!;
            var mup = view.AddComponent<MouseUpHandlerComponent>()!;
            mup.Keys = MouseKey.Left;
            component.Color = _color;
            component.Wrapping = _wrapping;
            component.Size = _size;
            component.Text = _content;
            component.FontStyle = _fontStyle;
            component.Update();
        }

        public static implicit operator TMP_Text(Text text) => text._textComponent;

    }
}
