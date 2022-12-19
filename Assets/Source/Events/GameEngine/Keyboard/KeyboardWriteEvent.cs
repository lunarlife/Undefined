using Utils.Events;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardWriteEvent: Event
    {
        public string Input {get; }

        public KeyboardWriteEvent(string input)
        {
            Input = input;
        }
    }
}