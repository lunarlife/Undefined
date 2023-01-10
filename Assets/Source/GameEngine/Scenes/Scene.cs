using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using GameEngine.GameObjects.Core;
using UndefinedNetworking.Events.ObjectEvents;
using UndefinedNetworking.Events.SceneEvents;
using UndefinedNetworking.Events.UIEvents;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.Objects;
using UndefinedNetworking.GameEngine.Scenes.UI;
using Utils.Events;

namespace GameEngine.Scenes
{
    public class Scene : IScene
    {
        private readonly Dictionary<uint, IObjectBase> _objects = new();

        public Event<SceneUnloadEventData> SceneUnload { get; } = new();
        public Event<UIOpenEventData> UIOpen { get; } = new();
        public Scene(ISceneViewer viewer)
        {
            Viewer = viewer;
            Type = SceneType.XYZ;
        }

        public ISceneViewer Viewer { get; }
        public IObjectBase[] Objects => _objects.Values.ToArray();
        public SceneType Type { get; }

        public void Unload()
        {
            SceneUnload.Invoke(new SceneUnloadEventData(this));
        }

        public IUIView OpenView(ViewParameters parameters)
        {
            var view = new UIView(Viewer, parameters);
            _objects.Add(view.Identifier, view);
            view.OnClose.AddListener(OnCloseView);
            UIOpen.Invoke(new UIOpenEventData(view));
            return view;
        }

        private void OnCloseView(UICloseEventData data)
        {
            if (!Objects.Contains(data.View)) throw new ObjectException("unknown object");
            _objects.Remove(data.View.Identifier);
        }

        public void DestroyObject(IGameObject obj)
        {
            if (!Objects.Contains(obj)) throw new ObjectException("unknown object");
            _objects.Remove(obj.Identifier);
        }

        public IUIView GetView(uint identifier)
        {
            var b = !_objects.ContainsKey(identifier);
            if (b) return null;

            var objectBase = _objects[identifier];
            return objectBase as IUIView ?? throw new ViewException("view not found");
        }

        public bool TryGetView(uint identifier, out IUIView? view)
        {
            if (!_objects.ContainsKey(identifier) || _objects[identifier] is not IUIView v)
            {
                view = null;
                return false;
            }

            view = v;
            return true;
        }


        public IUIView OpenNetworkView(uint identifier)
        {
            var view = new NetworkUIView(Viewer, identifier);
            _objects.Add(view.Identifier, view);
            UIOpen.Invoke(new UIOpenEventData(view));
            return view;
        }
    }
}