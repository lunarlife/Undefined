using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using GameEngine.GameObjects.Core.Unity;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.UI;
using UnityEngine;
using Utils.Events;
using Utils.Pool;
using Object = UnityEngine.Object;

namespace GameEngine.GameObjects.Core
{
    public abstract class ObjectCore : IDisposable
    {

        private static readonly ObjectPool<UnityObject> GameObjects = new();
        private static readonly ObjectPool<UnityObject> UIObjects = new();
        
        protected readonly UnityObject Instance;
        private string _name;
        private bool _isActive;
        private ObjectCore _parent;
        private readonly List<Component> _unityComponents = new();
        public bool IsDestroyed { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value ?? "";
                Undefined.CallSynchronously(() => Instance.name = $"{{{GetType().Name}}}{(string.IsNullOrEmpty(_name) ? "" : "=>" + _name)}");
            }
        }

        public ObjectCore(string name = "")
        {
            var pool = this is IUIView ? UIObjects : GameObjects;
            if (!pool.TryTake(out Instance)) throw new ObjectException("no objects in pool");
            Instance.Object = this;
            Name = name;
        }
        protected abstract void DoDestroy();


        public T GetUnityComponent<T>() where T : Component => GetUnityComponent(typeof(T)) as T;
        public T AddUnityComponent<T>() where T : Component => AddUnityComponent(typeof(T)) as T;
        public T GetOrAddUnityComponent<T>() where T : Component =>
            GetUnityComponent<T>() ?? AddUnityComponent<T>();

        public Component GetUnityComponent(Type type) => _unityComponents.FirstOrDefault(c => c.GetType() == type);
        public Component AddUnityComponent(Type type)
        {
            if (type == null) throw new ComponentException("type cant be null");
            if (GetUnityComponent(type) != null)
                throw new ComponentException("component is already exists");
            var component = Instance.gameObject.AddComponent(type);
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
                if(Instance is UnityUIObject)
                    UIObjects.Return(Instance);
                else if(Instance is UnityGameObject)
                    GameObjects.Return(Instance);
            });
        }
        public static void CreateGameObjectsPoolInstances(int count)
        {
            var unityObjects = new UnityGameObject[count];
            for (var i = 0; i < count; i++)
            {
                var gameObject = new UnityEngine.GameObject();
                unityObjects[i] = gameObject.AddComponent<UnityGameObject>();
            }
            GameObjects.Return(unityObjects);
        }

        public static void CreateUIPoolInstances(int count)
        {
            var unityObjects = new UnityUIObject[count];
            for (var i = 0; i < count; i++)
            {
                var gameObject = new UnityEngine.GameObject();
                unityObjects[i] = gameObject.AddComponent<UnityUIObject>();
            }
            UIObjects.Return(unityObjects);
        }

       
        public void Destroy()
        {
            Dispose();
        }
        public void Dispose()
        {
            DestroyObject();
        }
    }
}