using UndefinedNetworking.Chats;
using Utils.Events;

namespace Events.Chat
{
    public class MessageAddEvent: Event
    {
        public ChatMessage ChatMessage { get; }

        public MessageAddEvent(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}