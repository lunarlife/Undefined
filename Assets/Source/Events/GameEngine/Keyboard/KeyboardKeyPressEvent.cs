using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyPressEvent : KeyboardEvent
    {
        public KeyboardKeyPressEvent(KeyboardKey key)
        {
            Key = key;
        }

        public override KeyboardKey Key { get; }
    }
}