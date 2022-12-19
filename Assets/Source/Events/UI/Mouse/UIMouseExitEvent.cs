using GameEngine.GameObjects.Core;

namespace Events.UI.Mouse
{

    public class UIMouseExitEvent : UIMouseEvent
    {
        public override UIView View { get; }

        public UIMouseExitEvent(UIView view)
        {
            View = view;
        }
    }
}