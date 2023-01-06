using System.IO;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Resources;
using UnityEngine;

namespace GameEngine.Resources
{
    public class Sprite : ISprite
    {
        public Sprite(string path, int id)
        {
            Path = path;
            if (!File.Exists(FullPath)) throw new SpriteException("file not exists");
            if (!path.EndsWith(".png")) throw new SpriteException("unknown file extension");
            Id = id;
            Undefined.CallSynchronously(() =>
            {
                var texture = new Texture2D(0, 0);
                texture.LoadImage(File.ReadAllBytes(FullPath));
                Width = texture.width;
                Height = texture.height;
                UnitySprite = UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                    Vector2.zero);
            });
        }

        public UnityEngine.Sprite UnitySprite { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Path { get; }
        public string FullPath => System.IO.Path.Join(Paths.ExternalResources, Path);
        public int Id { get; }
    }
}