using GameEngine.Components;
using UECS;
using UnityEngine;
using UnityEngine.UI;

namespace GameEngine.UI.Systems
{
    public class CanvasSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<CanvasComponent> _canvasFilter;
        [ChangeHandler] private Filter<CanvasScalerComponent> _canvasScalerFilter;
        
        public void Init()
        {
            
        }

        public void Update()
        {
            foreach (var result in _canvasScalerFilter)
            {
                var component = result.Get1();
                if((CanvasScaler)component.Component is not { } scaler) continue;
                scaler.uiScaleMode = component.ScaleMode;
                scaler.referenceResolution = component.ReferenceResolution.ToUnityVector();
                scaler.matchWidthOrHeight = component.MatchWidthOrHeight;
                scaler.referencePixelsPerUnit = component.ReferencePixelsPerUnit;
                scaler.screenMatchMode = component.ScreenMatchMode;
            }
            foreach (var result in _canvasFilter)
            {
                var component = result.Get1();
                if((Canvas)component.Component is not { } canvas) continue;
                canvas.worldCamera = (Camera)component.Camera.Component;
                canvas.planeDistance = component.PlaneDistance;
                canvas.renderMode = component.RenderMode;
            }
        }
    }
}