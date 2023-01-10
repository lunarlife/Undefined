using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Shader = GameEngine.Resources.Shader;
using Sprite = GameEngine.Resources.Sprite;

namespace GameEngine.UI.Systems
{
    public class ImageSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<ImageComponent>> _filter;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _filter)
            {
                result.Get1().Read(component =>
                {
                    var image = ((ObjectCore)component.TargetObject).GetOrAddUnityComponent<Image>();
                    // ReSharper disable once AssignNullToNotNullAttribute
                    image.sprite = (component.Sprite as Sprite)?.UnitySprite;
                    if (component.Shader is { } sh)
                    {
                        var shader = ((Shader)sh).UnityShader;
                        image.material = image.material.shader.name == shader.name ? image.material : new Material(shader);
                    }
                    if (component.FilledSettings is not { } filledSettings) return;
                    image.fillAmount = filledSettings.FillAmount;
                    image.fillMethod = filledSettings.FillMethod.ToUnityFillMethod();
                    image.fillOrigin = (int)filledSettings.FillOrigin;
                });
                
            }
        }
    }
}