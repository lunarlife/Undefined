using GameEngine.GameObjects.Core;

namespace Events.UI.Mouse
{

    public class UIMouseHoldingEvent : UIMouseEvent
    {
        public override UIView View { get; }

        public UIMouseHoldingEvent(UIView view)
        {
            View = view;
        }
    }
}