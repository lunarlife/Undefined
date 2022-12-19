using UndefinedNetworking.GameEngine.Input;
using Event = Utils.Events.Event;

namespace Events.GameEngine.Keyboard
{
    public abstract class KeyboardEvent : Event
    {
        public abstract KeyboardKey Key { get; }
    }
}