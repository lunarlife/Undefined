using System;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;

namespace GameEngine.Components
{
    public record CameraComponent : UIComponentData
    {
        public bool Orthographic { get; set; }

        public float OrthographicSize { get; set; }

        public float NearClipPlane { get; set; }

        public float FarClipPlane { get; set; }
    }
}