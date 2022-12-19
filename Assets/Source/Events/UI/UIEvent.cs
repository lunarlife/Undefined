using GameEngine.GameObjects.Core;
using Utils.Events;

namespace Events.UI
{
    public abstract class UIEvent : Event
    {
        public abstract UIView View { get; }
    }
}