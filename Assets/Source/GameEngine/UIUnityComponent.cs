using System;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;

namespace GameEngine
{
    public abstract record UIUnityComponent : UIComponent
    {
        public abstract Type ComponentType { get; }
        public Component Component { get; private set; }
    }
}