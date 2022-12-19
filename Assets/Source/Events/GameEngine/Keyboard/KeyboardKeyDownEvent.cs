using UndefinedNetworking.GameEngine.Input;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardKeyDownEvent : KeyboardEvent
    {
        public override KeyboardKey Key { get; }
        public KeyboardKeyDownEvent(KeyboardKey key)
        {
            Key = key;
        }
    }
}