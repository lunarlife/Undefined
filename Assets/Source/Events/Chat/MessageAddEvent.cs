using UndefinedNetworking.Chats;
using Utils.Events;

namespace Events.Chat
{
    public class MessageAddEvent : Event
    {
        public MessageAddEvent(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }

        public ChatMessage ChatMessage { get; }
    }
}