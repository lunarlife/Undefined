using GameEngine.Components;
using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UnityEngine;

namespace GameEngine.UI.Systems
{
    public class CameraSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<CameraComponent>> _cameraChanged;


        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _cameraChanged)
            {
                result.Get1().Read(component =>
                {
                    var camera = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<Camera>();
                    camera.orthographic = component.Orthographic;
                    camera.orthographicSize = component.OrthographicSize;
                    camera.farClipPlane = component.FarClipPlane;
                    camera.nearClipPlane = component.NearClipPlane;
                });
            }
        }
    }
}