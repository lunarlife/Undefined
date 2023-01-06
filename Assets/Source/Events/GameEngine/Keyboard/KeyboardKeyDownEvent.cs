using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyDownEvent : KeyboardEvent
    {
        public KeyboardKeyDownEvent(KeyboardKey key)
        {
            Key = key;
        }

        public override KeyboardKey Key { get; }
    }
}