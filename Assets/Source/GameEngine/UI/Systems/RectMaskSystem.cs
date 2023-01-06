using GameEngine.GameObjects.Core;
using GameEngine.Resources;
using UECS;
using UndefinedNetworking.GameEngine.Scenes.UI.Components.RectMask;
using UnityEngine;
using UnityEngine.UI;
using Shader = UnityEngine.Shader;

namespace GameEngine.UI.Systems
{
    public class RectMaskSystem : ISyncSystem
    {
        private static readonly int MaskShaderProperty = Shader.PropertyToID("_Mask");
        private static readonly int WidthShaderProperty = Shader.PropertyToID("_Width");
        private static readonly int HeightShaderProperty = Shader.PropertyToID("_Height");

        [ChangeHandler] private Filter<ObjectRectMaskComponent> _objectMasks;
        [ChangeHandler] private Filter<WorldRectMaskComponent> _worldMasks;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _worldMasks)
            {
                var component = result.Get1();
                var image = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<Image>();
                var shader = Undefined.ServerManager.InternalResourcesManager
                    .GetInternalShader(InternalShaderType.WorldRectMask).UnityShader;
                var material = image.material.shader.name == shader.name ? image.material : new Material(shader);
                material.SetVector(MaskShaderProperty, component.ViewRect.ToUnityVector());
                image.material = material;
            }

            foreach (var result in _objectMasks)
            {
                var component = result.Get1();
                var transform = component.TargetView.Transform;
                var image = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<Image>();
                var shader = Undefined.ServerManager.InternalResourcesManager
                    .GetInternalShader(InternalShaderType.ObjectRectMask).UnityShader;
                var material = image.material.shader.name == shader.name ? image.material : new Material(shader);
                material.SetVector(MaskShaderProperty, component.ViewRect.ToUnityVector());
                material.SetFloat(WidthShaderProperty, transform.AnchoredRect.Width);
                material.SetFloat(HeightShaderProperty, transform.AnchoredRect.Height);
                image.material = material;
            }
        }
    }
}