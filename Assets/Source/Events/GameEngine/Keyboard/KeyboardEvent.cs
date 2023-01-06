using UndefinedNetworking.GameEngine.Input;
using Utils.Events;

namespace Events.GameEngine.Keyboard
{
    public abstract class KeyboardEvent : Event
    {
        public abstract KeyboardKey Key { get; }
    }
}