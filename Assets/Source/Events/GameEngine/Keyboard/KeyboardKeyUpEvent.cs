using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyUpEvent : KeyboardEvent
    {
        public KeyboardKeyUpEvent(KeyboardKey key)
        {
            Key = key;
        }

        public override KeyboardKey Key { get; }
    }
}