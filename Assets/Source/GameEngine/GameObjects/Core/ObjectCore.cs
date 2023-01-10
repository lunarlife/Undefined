using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using GameEngine.GameObjects.Core.Unity;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Views;
using UnityEngine;
using Utils.Pool;
using Object = UnityEngine.Object;

namespace GameEngine.GameObjects.Core
{
    public abstract class ObjectCore : IDisposable
    {
        private static readonly ObjectPool<UnityObject> GameObjects = new();
        private static readonly ObjectPool<UnityObject> UIObjects = new();
        private readonly List<Component> _unityComponents = new();

        protected readonly UnityObject Instance;
        private bool _isActive;
        private readonly List<ObjectCore> _localChilds = new();
        private string _name;
        private ObjectCore _parent;

        public ObjectCore(string name = "")
        {
            var pool = this is IUIView ? UIObjects : GameObjects;
            if (!pool.TryTake(out Instance)) throw new ObjectException("no objects in pool");
            Instance.Object = this;
            Name = name;
        }

        public bool IsDestroyed { get; private set; }

        public ObjectCore LocalParent
        {
            get => _parent;
            set
            {
                _parent?._localChilds.Remove(this);
                Instance.transform.SetParent((_parent = value).GetUnityComponent<Transform>());
                _parent?._localChilds.Add(this);
            }
        }

        public IReadOnlyList<ObjectCore> LocalChilds => _localChilds;

        public string Name
        {
            get => _name;
            set
            {
                _name = value ?? "";
                Undefined.CallSynchronously(() =>
                    Instance.name = $"{{{GetType().Name}}}{(string.IsNullOrEmpty(_name) ? "" : "=>" + _name)}");
            }
        }

        public void Dispose()
        {
            DestroyObject();
        }

        protected abstract void DoDestroy();


        public T GetUnityComponent<T>() where T : Component
        {
            return GetUnityComponent(typeof(T)) as T;
        }

        public T AddUnityComponent<T>() where T : Component
        {
            return AddUnityComponent(typeof(T)) as T;
        }

        public T GetOrAddUnityComponent<T>() where T : Component
        {
            return GetUnityComponent<T>() ?? AddUnityComponent<T>();
        }

        public Component GetUnityComponent(Type type)
        {
            var component = _unityComponents.FirstOrDefault(c => c.GetType() == type);
            if (!component)
            {
                component = Instance.GetComponent(type);
                if (component)
                    _unityComponents.Add(component);
                else component = null;
            }

            return component;
        }

        public Component AddUnityComponent(Type type)
        {
            if (type == null) throw new ComponentException("type cant be null");
            if (GetUnityComponent(type) != null)
                throw new ComponentException("component is already exists");
            var c = Instance.gameObject.GetComponent(type);
            var component = c ? c : Instance.gameObject.AddComponent(type);
            if (component is not null)
                _unityComponents.Add(component);
            return component;
        }

        private void DestroyObject()
        {
            IsDestroyed = true;
            DoDestroy();
            Undefined.CallSynchronously(() =>
            {
                foreach (var component in _unityComponents) Object.Destroy(component);
                Instance.Object = null;
                Instance.transform.parent = UnityObject.PoolObjectsParent;
                if (Instance is UnityUIObject)
                    UIObjects.Return(Instance);
                else if (Instance is UnityGameObject)
                    GameObjects.Return(Instance);
            });
        }

        public static void CreateGameObjectsPoolInstances(int count)
        {
            var unityObjects = new UnityGameObject[count];
            for (var i = 0; i < count; i++)
            {
                var gameObject = new GameObject();
                unityObjects[i] = gameObject.AddComponent<UnityGameObject>();
            }

            GameObjects.Return(unityObjects);
        }

        public static void CreateUIPoolInstances(int count)
        {
            var unityObjects = new UnityUIObject[count];
            for (var i = 0; i < count; i++)
            {
                var gameObject = new GameObject();
                unityObjects[i] = gameObject.AddComponent<UnityUIObject>();
            }

            UIObjects.Return(unityObjects);
        }


        public void Destroy()
        {
            Dispose();
        }
    }
}