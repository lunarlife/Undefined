using UndefinedNetworking.Chats;
using Utils.Events;

namespace Events.Chat
{
    public class ChatMessageReceivedEvent: Event
    {
        public ChatMessage ChatMessage { get; }

        public ChatMessageReceivedEvent(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}