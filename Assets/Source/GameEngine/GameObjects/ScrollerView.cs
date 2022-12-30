using Events.Tick;
using Utils.Events;
using Rect = Utils.Rect;

namespace GameEngine.GameObjects
{
    //TODO: update to new system
    public class ScrollerView 
    {
        private Panel _content;
        private Rect _viewRect;
        public bool IsHorizontalScroll { get; set; }
        public bool IsVerticalScroll { get; set; }
        public float HorizontalScrollSpeed { get; set; }
        public float VerticalScrollSpeed { get; set; }
        
        /*public ScrollerView(Rect rect, Rect viewRect, bool isHorizontalScroll, bool isVerticalScroll, float horizontalScrollSpeed, float verticalScrollSpeed, Sprite background = null, Color? backgroundColor = null, UIBind? bind = null, Side pivot = Side.Center, int layer = 0, UIElement parent = null, string name = "", bool isActive = true) 
            : base(new Rect(rect.Position, viewRect.Width, viewRect.Height), Margins.Zero, name, bind, pivot, layer, parent, isActive)
        {
            _viewRect = viewRect;
            _content = new Panel(new Rect(viewRect.Position, rect.Width, rect.Height), sprite: background, color:backgroundColor ?? Color.Transparent, bind: null, pivot: Side.Top, layer: layer, parent: this, name: name);
            IsHorizontalScroll = isHorizontalScroll;
            IsVerticalScroll = isVerticalScroll;
            HorizontalScrollSpeed = horizontalScrollSpeed;
            VerticalScrollSpeed = verticalScrollSpeed;
            ViewRect = viewRect;
        }*/

        public void SetRect(Rect rect) // todo: rename
        {
            //_content.Transform.OriginalRect = rect;
            UpdateRect();
        }

        /*
        public void AddElement(UIElement element)
        {
            /*element.Transform.Parent = _content.Transform#1#
        }*/

        [EventHandler]
        private void OnAsyncTick(AsyncTickEvent e)
        {
            return;
            //if(!Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) return;
            //if(!Engine.RaycastUISynchronously(_content, Engine.MouseScreenPositionUnscaled, true)) return;
            UpdateRect();
        }

        private void UpdateRect()
        {
            /*var currentRect = _content.Transform.OriginalRect;
            var move = Dot2.Zero;
            if (IsVerticalScroll) move.Y = Undefined.MouseScroll * VerticalScrollSpeed;
            if (IsHorizontalScroll) move.X = Undefined.MouseScroll * HorizontalScrollSpeed;
            var contentPosition = currentRect.Position;
            var pos = new Dot2(
                MathUtils.ClampOut(contentPosition.X - move.X, Transform.AnchoredRect.Width - currentRect.Width,
                    ViewRect.Position.X),
                MathUtils.ClampOut(contentPosition.Y - move.Y, Transform.AnchoredRect.Height - currentRect.Height,
                    ViewRect.Position.Y));
            _content.Transform.OriginalRect = new Rect(pos, currentRect.Width, currentRect.Height);*/
        }

        public Rect ViewRect { get; set; }
    }
}