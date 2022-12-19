using GameEngine.GameObjects.Core;

namespace Events.UI
{
    public class UICloseEvent : UIEvent
    {
        public override UIView View { get; }

        public UICloseEvent(UIView view)
        {
            View = view;
        }
    }
}