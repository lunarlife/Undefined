using System;
using UnityEngine.UI;

namespace GameEngine.Components
{
    public record GraphicRaycasterComponent : UIUnityComponent
    {
        public override Type ComponentType { get; } = typeof(GraphicRaycaster);
    }
}