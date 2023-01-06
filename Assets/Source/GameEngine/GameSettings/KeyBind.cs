using UndefinedNetworking.GameEngine.Input;

namespace GameEngine.GameSettings
{
    public struct KeyBind
    {
        public KeyboardKey[] Keys { get; set; }
        public ClickState Type { get; set; }

        public static implicit operator KeyBind(KeyboardKey code)
        {
            return new()
            {
                Keys = new[] { code }
            };
        }

        public static implicit operator KeyboardKey[](KeyBind bind)
        {
            return bind.Keys;
        }

        public bool IsPressed(ClickState state = ClickState.All)
        {
            return Type == ClickState.All ? Undefined.IsPressed(Keys, state) : Undefined.IsPressedAny(Keys, state);
        }
    }
}