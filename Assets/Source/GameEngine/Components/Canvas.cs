using System;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;

namespace GameEngine.Components
{
    public record Canvas : UIComponentData
    {
        public float PlaneDistance { get; set; }

        public RenderMode RenderMode { get; set; }

        public IComponent<CameraComponent> Camera { get; set; }

        public float ScaleFactor { get; set; }
    }
}