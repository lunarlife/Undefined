using System;
using UndefinedNetworking.GameEngine.UI.Components;

namespace GameEngine
{
    public abstract record UIUnityComponentAdapter : UIComponent
    {
        public abstract Type ComponentType { get; }
        public UnityEngine.Component Component { get; private set; }
    }
}