using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;
using Rect = UnityEngine.Rect;

namespace GameData
{
    public static class Data
    {
        private static bool _isLoaded;
        private static Dictionary<string, Sprite> _sprites = new();
        public static Version Version { get; } = new("0.1alpha");

        public static void Load()
        {
            if (_isLoaded) throw new DataException("Data is load(dead)");

            AddTextureInResources("chat", "Chat/Chat");
            AddTextureInResources("chat_type_but", "Chat/Button");
            AddTextureInResources("chat_avatar_frame", "Chat/FrameAvatar");
            AddTextureInResources("chat_standard_avatar", "Chat/StandardAvatar");
            AddTextureInResources("chat_message_panel", "Chat/MessagePanel");
            _isLoaded = true;
        }

        private static void AddTextureInResources(string name, string path)
        {
            var texture2D = Resources.Load<Texture2D>(path);
            var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
            _sprites.Add(name,sprite); 
        }



        public static Sprite GetSprite(string name)
        {
            if (!_sprites.ContainsKey(name)) throw new DataException($"Dict has not {name} texture");
            return _sprites[name];
        }
    }
}