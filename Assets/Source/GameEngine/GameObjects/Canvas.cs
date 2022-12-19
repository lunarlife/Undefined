using System;
using System.Collections.Generic;
using GameEngine.Components;
using GameEngine.GameObjects.Core;
using GameEngine.GameSettings;
using UndefinedNetworking.GameEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.Dots;

namespace GameEngine.GameObjects
{
    public class Canvas : UIElement
    {
        private float _planeDistance;
        private readonly CameraComponent _camera;

        public Canvas(float planeDistance = 10, CameraComponent camera = null)
        {
            _planeDistance = planeDistance;
            _camera = camera;
        }
        

        public bool RaycastSynchronously(UIElement element, Dot2Int position, bool onlyFirst = false)
        {
            return true;
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = position.ToUnityVector(),
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            /*return onlyFirst ? element.CompareWithUnityObjectSynchronously(results.FirstOrDefault().gameObject) : 
                results.Any(r => element.CompareWithUnityObjectSynchronously(r.gameObject));*/
        }

        public void RaycastAsync(UIElement element, Dot2Int position, bool onlyFirst, Action<bool> callback)
        {
            Undefined.CallSynchronously(ActionCallback.Of(() =>
            {
                var eventData = new PointerEventData(EventSystem.current)
                {
                    position = position.ToUnityVector()
                };
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);
                return true;
                /*return onlyFirst ? element.CompareWithUnityObjecxtSynchronously(results.FirstOrDefault().gameObject) : 
                    results.Any(r => element.CompareWithUnityObjectSynchronously(r.gameObject));*/
            }, callback));
        }

        public override void OnCreateView(IUIView view)
        {
            var component = view.AddComponent<CanvasComponent>()!;
            component.PlaneDistance = _planeDistance;
            if (_camera is not null)
            {
                component.Camera = _camera;
                component.RenderMode = RenderMode.ScreenSpaceCamera;
            }
            view.AddComponents<CanvasScalerComponent, GraphicRaycasterComponent>();
        }

        public override ViewParameters CreateNewView(IUIViewer viewer) => new()
        {
            IsActive = true,
            OriginalRect = Settings.ResolutionUnscaled,
        };

            /*_canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _canvasScaler.referenceResolution = new Vector2(1920, 1080);
            _canvasScaler.referencePixelsPerUnit = 100;
            _canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            _canvasScaler.matchWidthOrHeight = .5f;*/
    }
}