using System;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine.UI;
using Utils.Dots;

namespace GameEngine.Components
{
    public record CanvasScalerComponent : UIComponentData
    {
        public CanvasScaler.ScaleMode ScaleMode { get; set; }

        public Dot2 ReferenceResolution { get; set; }

        public float ReferencePixelsPerUnit { get; set; }

        public CanvasScaler.ScreenMatchMode ScreenMatchMode { get; set; }

        public float MatchWidthOrHeight { get; set; }
    }
}