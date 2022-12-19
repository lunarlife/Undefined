using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyUpEvent : KeyboardEvent
    {
        public override KeyboardKey Key { get; }
        public KeyboardKeyUpEvent(KeyboardKey key)
        {
            Key = key;
        }
    }
}