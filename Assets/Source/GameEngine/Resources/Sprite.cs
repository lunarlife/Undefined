using System.IO;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Resources;
using UnityEngine;

namespace GameEngine.Resources
{
    public class Sprite : ISprite
    {
        public int Width { get; }
        public int Height { get; }
        public string ResourcePath { get; }
        public string Name { get; }
        public UnityEngine.Sprite UnitySprite { get; }
        
        public Sprite(string path, string name)
        {
            if (!File.Exists(path)) throw new SpriteException("file not exists");
            if (!path.EndsWith(".png")) throw new SpriteException("unknown file extension");
            var texture = new Texture2D(0, 0);
            texture.LoadImage(File.ReadAllBytes(path));
            ResourcePath = path;
            Name = name;
            Width = texture.width;
            Height = texture.height;
            UnitySprite = UnityEngine.Sprite.Create(texture, new Rect(0,0, texture.width, texture.height), Vector2.zero);
        }
    }
}