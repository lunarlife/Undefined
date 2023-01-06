using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using GameEngine.GameObjects.Core;
using Networking;
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

        public Scene(ISceneViewer viewer)
        {
            Viewer = viewer;
            Type = SceneType.XYZ;
            EventManager.CallEvent(new SceneLoadEvent(this));
        }

        public ISceneViewer Viewer { get; }
        public IObjectBase[] Objects => _objects.Values.ToArray();
        public SceneType Type { get; }

        public void Unload()
        {
            EventManager.CallEvent(new SceneUnloadEvent(this));
        }

        public IUIView OpenView(ViewParameters parameters)
        {
            var view = new UIView(Viewer, parameters);
            _objects.Add(view.Identifier, view);
            EventManager.CallEvent(new UIOpenEvent(view));
            return view;
        }

        public void CloseView(IUIView view)
        {
            if (!Objects.Contains(view)) throw new ObjectException("unknown object");
            _objects.Remove(view.Identifier);
            EventManager.CallEvent(new UICloseEvent(view));
        }

        public void DestroyObject(IGameObject obj)
        {
            if (!Objects.Contains(obj)) throw new ObjectException("unknown object");
            _objects.Remove(obj.Identifier);
            EventManager.CallEvent(new ObjectDestroyEvent(obj));
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
            EventManager.CallEvent(new UIOpenEvent(view));
            return view;
        }
    }
}