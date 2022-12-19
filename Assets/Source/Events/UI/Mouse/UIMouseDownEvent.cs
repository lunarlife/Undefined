using GameEngine.GameObjects.Core;

namespace Events.UI.Mouse
{

    public class UIMouseDownEvent : UIMouseEvent
    {
        public override UIView View { get; }

        public UIMouseDownEvent(UIView view)
        {
            View = view;
        }
    }
}
