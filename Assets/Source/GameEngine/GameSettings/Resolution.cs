using Utils;

namespace GameEngine.GameSettings
{
    public struct Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsFullScreen { get; set; }

        public override string ToString()
        {
            return $"{Width}x{Height}";
        }

        public static implicit operator Rect(Resolution res)
        {
            return new(0, 0, res.Width, res.Height);
        }
    }
}