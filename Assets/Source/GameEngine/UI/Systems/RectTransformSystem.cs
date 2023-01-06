using GameEngine.GameObjects.Core;
using UECS;
using UnityEngine;
using RectTransform = UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform;

namespace GameEngine.UI.Systems
{
    public class RectTransformSystem : ISyncSystem
    {
        [ChangeHandler(true)] private Filter<RectTransform> _filter;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _filter)
            {
                var component = result.Get1();
                var view = component.TargetView;
                var objectCore = (ObjectCore)view;
                var transform = objectCore.GetOrAddUnityComponent<UnityEngine.RectTransform>();
                if (view == Undefined.Camera ||
                    view == Undefined.Canvas)
                    transform.SetParent(null);
                else if (objectCore.LocalParent == null)
                    transform.SetParent(((component.Parent ?? Undefined.Canvas.Transform)?.TargetView as ObjectCore)
                        ?.GetOrAddUnityComponent<UnityEngine.RectTransform>());
                transform.anchorMin = Vector2.zero;
                transform.anchorMax = Vector2.zero;
                transform.localScale = Vector3.one;
                transform.pivot = component.PivotValue.ToUnityVector();
                transform.localPosition = new Vector3(0, 0, -component.Layer);
                transform.anchoredPosition = component.LocalRect.Position.ToUnityVector();
                transform.sizeDelta = component.LocalRect.WidthHeight.ToUnityVector();
                transform.gameObject.SetActive(component.IsActive);
            }
        }
    }
}