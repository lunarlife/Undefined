using GameEngine.Components;
using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UnityEngine;
using UnityEngine.UI;
using Canvas = GameEngine.Components.Canvas;

namespace GameEngine.UI.Systems
{
    public class CanvasSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<Canvas>> _canvasFilter;
        [ChangeHandler] private Filter<IComponent<CanvasScalerComponent>> _canvasScalerFilter;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _canvasScalerFilter)
            {
                result.Get1().Read(component =>
                {
                    var scaler = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<CanvasScaler>();
                    scaler.uiScaleMode = component.ScaleMode;
                    scaler.referenceResolution = component.ReferenceResolution.ToUnityVector();
                    scaler.matchWidthOrHeight = component.MatchWidthOrHeight;
                    scaler.referencePixelsPerUnit = component.ReferencePixelsPerUnit;
                    scaler.screenMatchMode = component.ScreenMatchMode;
                });
            }

            foreach (var result in _canvasFilter)
            {
                result.Get1().Read(component =>
                {
                    var canvas = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<UnityEngine.Canvas>();
                    component.Camera.Read(cameraRead =>
                    {
                        canvas.worldCamera = ((ObjectCore)cameraRead.TargetView).GetOrAddUnityComponent<Camera>();
                    });
                    canvas.planeDistance = component.PlaneDistance;
                    canvas.renderMode = component.RenderMode;
                });
            }
        }
    }
}