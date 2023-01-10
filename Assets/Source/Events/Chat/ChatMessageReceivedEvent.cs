using UndefinedNetworking.Chats;
using Utils.Events;

namespace Events.Chat
{
    public class ChatMessageReceivedEvent : EventData
    {
        public ChatMessageReceivedEvent(ChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }

        public ChatMessage ChatMessage { get; }
    }
}