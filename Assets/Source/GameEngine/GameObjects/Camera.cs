using GameEngine.Components;
using GameEngine.GameObjects.Core;
using UndefinedNetworking.GameEngine.UI;
using Utils.Dots;

namespace GameEngine.GameObjects
{
    public class Camera : UIElement
    {
        private readonly bool _orthographic;
        private readonly float _size;
        private readonly float _far;
        private readonly float _near;

        public Camera(bool orthographic = true, float size = 10, float far = 40, float near = .3f, Dot2? position = null, Dot2? scale = null,
            float rotation = 0, string name = "", UIElement parent = null, bool isActive = true) : base(parent)
        {
            _orthographic = orthographic;
            _size = size;
            _far = far;
            _near = near;
        }

        public override void OnCreateView(IUIView view)
        {
            var component = view.AddComponent<CameraComponent>()!;
            component.Orthographic = _orthographic;
            component.OrthographicSize = _size;
            component.FarClipPlane = _far;
            component.NearClipPlane = _near;
        }

        public override ViewParameters CreateNewView(IUIViewer viewer) => new()
        {
            IsActive = true
        };
    }
}