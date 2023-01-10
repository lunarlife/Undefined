using Utils.Events;

namespace Events.GameEngine.Keyboard
{
    public class KeyboardWriteEvent : EventData
    {
        public KeyboardWriteEvent(string input)
        {
            Input = input;
        }

        public string Input { get; }
    }
}