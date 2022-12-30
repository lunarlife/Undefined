using System;
using UnityEngine;

namespace GameEngine.Components
{
    public record CameraComponent : UIUnityComponent
    {
        private bool _orthographic;
        private float _orthographicSize;
        private float _nearClipPlane;
        private float _farClipPlane;
        public override Type ComponentType { get; } = typeof(Camera);
        public bool Orthographic
        {
            get => _orthographic;
            set
            {
                _orthographic = value;
                Update();
            }
        }

        public float OrthographicSize
        {
            get => _orthographicSize;
            set
            {
                _orthographicSize = value;
                Update();
            }
        }

        public float NearClipPlane
        {
            get => _nearClipPlane;
            set
            {
                _nearClipPlane = value;
                Update();
            }
        }

        public float FarClipPlane
        {
            get => _farClipPlane;
            set
            {
                _farClipPlane = value;
                Update();
            }
        }
    }
}