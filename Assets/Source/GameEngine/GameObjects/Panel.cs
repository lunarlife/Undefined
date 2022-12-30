using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using UnityEngine;
using UnityEngine.UI;
using Color = Utils.Color;
using Rect = Utils.Rect;

namespace GameEngine.GameObjects
{
    //TODO: update to new system
    public class Panel
    {
        private Image _image;
        private readonly Rect _rect;
        private readonly Margins _margins;
        private Sprite _sprite;
        private readonly UIBind _bind;
        private readonly Side _pivot;
        private readonly int _layer;
        private readonly bool _isActive;
        private Color _color;

        public Panel(Rect rect, Margins? margins = null, Sprite sprite = null, Color? color = null, UIBind? bind = null,
            Side pivot = Side.Center, int layer = 0, string name = "", bool isActive = true)
        {
            _rect = rect;
            _margins = margins ?? Margins.Zero;
            _sprite = sprite;
            _bind = bind ?? new UIBind();
            _pivot = pivot;
            _layer = layer;
            _isActive = isActive;
            _color = color ?? Color.White;
        }
        /*public override ViewParameters CreateNewView(IUIViewer viewer) => new()
        {
            OriginalRect = _rect,
            Margins = _margins,
            Layer = _layer,
            Pivot = _pivot,
            Bind = _bind,
            IsActive = _isActive
        };*/
    }
}