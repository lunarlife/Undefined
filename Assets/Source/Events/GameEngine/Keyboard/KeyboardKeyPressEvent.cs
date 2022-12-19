using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyPressEvent : KeyboardEvent
    {
        public override KeyboardKey Key { get; }
        public KeyboardKeyPressEvent(KeyboardKey key)
        {
            Key = key;
        }
    }
}