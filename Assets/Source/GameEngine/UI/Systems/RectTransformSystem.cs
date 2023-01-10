using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UnityEngine;
using UnityEngine.Events;
using RectTransform = UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform;

namespace GameEngine.UI.Systems
{
    public class RectTransformSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<RectTransform>> _filter;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _filter)
            {
                result.Get1().Read(component =>
                {
                    var view = component.TargetView;
                    var objectCore = (ObjectCore)view;
                    var transform = objectCore.GetOrAddUnityComponent<UnityEngine.RectTransform>();
                    if (view == Undefined.Camera ||
                        view == Undefined.Canvas)
                        transform.SetParent(null);
                    else if (objectCore.LocalParent == null)
                        (component.Parent ?? Undefined.Canvas.Transform)?.Read(parent =>
                        {
                            transform.SetParent(((ObjectCore)parent.TargetView)
                                .GetUnityComponent<UnityEngine.RectTransform>());
                        });
                    transform.anchorMin = Vector2.zero;
                    transform.anchorMax = Vector2.zero;
                    transform.localScale = Vector3.one;
                    transform.pivot = component.PivotValue.ToUnityVector();
                    transform.localPosition = new Vector3(0, 0, -component.Layer);
                    transform.anchoredPosition = component.LocalRect.Position.ToUnityVector();
                    transform.sizeDelta = component.LocalRect.WidthHeight.ToUnityVector();
                    transform.gameObject.SetActive(component.IsActiveUI);
                });
            }
        }
    }
}