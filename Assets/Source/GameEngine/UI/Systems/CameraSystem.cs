using GameEngine.Components;
using UECS;
using UnityEngine;

namespace GameEngine.UI.Systems
{
    public class CameraSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<CameraComponent> _cameraChanged;
        
        
        public void Init()
        {
            
        }

        public void Update()
        {
            foreach (var result in _cameraChanged)
            {
                var component = result.Get1();
                if((Camera)component.Component is not { } camera) continue;
                camera.orthographic = component.Orthographic;
                camera.orthographicSize = component.OrthographicSize;
                camera.farClipPlane = component.FarClipPlane;
                camera.nearClipPlane = component.NearClipPlane;
            }
        }
    }
}