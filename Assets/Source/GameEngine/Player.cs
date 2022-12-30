using GameEngine.Scenes;
using UndefinedNetworking.Chats;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.Gameplay;
using UnityEngine;

namespace GameEngine
{
    public class Player : MonoBehaviour, IPlayer
    {
        public IScene ActiveScene { get; private set; }
        public string Nickname { get; }
        public string SenderName { get; }
        public void SendMessage(ChatMessage message)
        {
            
        }
        public void LoadScene(SceneType type)
        {
            ActiveScene = new Scene(this);
        }
    }
}