using System;
using UnityEngine.UI;

namespace GameEngine.Components
{
    public record GraphicRaycasterComponent : UIUnityComponentAdapter
    {
        public override Type ComponentType { get; } = typeof(GraphicRaycaster);


    }
}