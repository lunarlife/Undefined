using GameEngine.GameObjects.Core;

namespace Events.UI.Mouse
{
    public class UIMouseEnterEvent : UIMouseEvent
    {
        public override UIView View { get; }
        public UIMouseEnterEvent(UIView view)
        {
            View = view;
        }
    }
}