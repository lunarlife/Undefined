using System;
using UnityEngine;

namespace GameEngine.Components
{
    public record CanvasComponent : UIUnityComponentAdapter
    {
        private float _planeDistance;
        private RenderMode _renderMode;
        private CameraComponent _camera;
        public override Type ComponentType { get; } = typeof(Canvas);

        public float PlaneDistance
        {
            get => _planeDistance;
            set
            {
                _planeDistance = value;
                Update();
            }
        }

        public RenderMode RenderMode
        {
            get => _renderMode;
            set
            {
                _renderMode = value;
                Update();
            }
        }
        public CameraComponent Camera
        {
            get => _camera;
            set
            {
                _camera = value;
                Update();
            }
        }
        public float ScaleFactor { get; set; }

    }
}