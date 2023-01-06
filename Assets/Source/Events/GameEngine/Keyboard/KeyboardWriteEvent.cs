using Utils.Events;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardWriteEvent : Event
    {
        public KeyboardWriteEvent(string input)
        {
            Input = input;
        }

        public string Input { get; }
    }
}