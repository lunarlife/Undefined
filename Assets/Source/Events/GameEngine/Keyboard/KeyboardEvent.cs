using UndefinedNetworking.GameEngine.Input;
using Utils.Events;

namespace Events.GameEngine.Keyboard
{
    public abstract class KeyboardEvent : EventData
    {
        public abstract KeyboardKey Key { get; }
    }
}