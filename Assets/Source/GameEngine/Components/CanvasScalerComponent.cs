using System;
using UnityEngine.UI;
using Utils.Dots;

namespace GameEngine.Components
{
    public record CanvasScalerComponent : UIUnityComponent
    {
        private float _matchWidthOrHeight;
        private float _referencePixelsPerUnit;
        private Dot2 _referenceResolution;
        private CanvasScaler.ScaleMode _scaleMode;
        private CanvasScaler.ScreenMatchMode _screenMatchMode;
        public override Type ComponentType { get; } = typeof(CanvasScaler);

        public CanvasScaler.ScaleMode ScaleMode
        {
            get => _scaleMode;
            set
            {
                _scaleMode = value;
                Update();
            }
        }

        public Dot2 ReferenceResolution
        {
            get => _referenceResolution;
            set
            {
                _referenceResolution = value;
                Update();
            }
        }

        public float ReferencePixelsPerUnit
        {
            get => _referencePixelsPerUnit;
            set
            {
                _referencePixelsPerUnit = value;
                Update();
            }
        }

        public CanvasScaler.ScreenMatchMode ScreenMatchMode
        {
            get => _screenMatchMode;
            set
            {
                _screenMatchMode = value;
                Update();
            }
        }

        public float MatchWidthOrHeight
        {
            get => _matchWidthOrHeight;
            set
            {
                _matchWidthOrHeight = value;
                Update();
            }
        }
    }
}