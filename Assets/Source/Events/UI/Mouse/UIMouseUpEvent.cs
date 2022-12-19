using GameEngine.GameObjects.Core;

namespace Events.UI.Mouse
{
    public class UIMouseUpEvent : UIMouseEvent
    {
        public override UIView View { get; }

        public UIMouseUpEvent(UIView view)
        {
            View = view;
        }
    }
}