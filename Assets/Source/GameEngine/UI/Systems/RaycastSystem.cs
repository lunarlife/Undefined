using System.Collections.Generic;
using System.Linq;
using GameEngine.Components;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Views;
using Utils.Dots;

namespace GameEngine.UI.Systems
{
    public sealed class RaycastSystem : ISyncSystem
    {
        [AutoInject] private Filter<IComponent<RectTransform>> _uis;
        private static RaycastSystem _instance;
        public void Init()
        {
            _instance = this;
        }

        public void Update()
        {
        }

        public static bool RaycastUIFirst(Dot2Int position, out IUIView cast)
        {
            RectTransform currentTarget = null;
            foreach (var result in _instance._uis)
            {
                var component = result.Get1();
                component.Read(transform =>
                {
                    if(transform.TargetObject.ContainsComponent<CameraComponent>() || transform.TargetObject.ContainsComponent<Canvas>()) return;
                    if (!transform.AnchoredRect.DotInRect(position)) return;
                    if (currentTarget is null || transform.Layer > currentTarget.Layer) currentTarget = transform;
                });
            }

            cast = (IUIView)currentTarget?.TargetObject;
            return cast is not null;
        }
        public static IEnumerable<IUIView> RaycastUIAll(Dot2Int position) =>
            _instance._uis.Select(result => result.Get1().CloneData)
                .Select(current => current.TargetObject.Transform.CloneData)
                .Where(transform => transform.AnchoredRect.DotInRect(position))
                .Select(transform => (IUIView)transform.TargetObject);
    }
}