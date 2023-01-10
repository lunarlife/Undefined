using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.GameObjects.Core.Unity;
using UndefinedNetworking.Events.UIEvents;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Views;
using Utils;
using Utils.Events;

namespace GameEngine.GameObjects.Core
{
    public sealed class NetworkUIView : ObjectCore, IUIView
    {
        private static readonly PropertyInfo TargetViewProperty = typeof(ComponentData).GetProperty("TargetObject",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        private readonly List<NetworkUIView> _childs;
        private readonly List<IComponent<UIComponentData>> _components = new();
        private readonly UnityEngine.RectTransform _unityTransform;

        public Event<UICloseEventData> OnClose { get; } = new();

        public NetworkUIView(ISceneViewer viewer, uint identifier)
        {
            Identifier = identifier;
            Viewer = viewer;
            _unityTransform = (Instance as UnityUIObject)!.Transform;
        }


        public IComponent<RectTransform> Transform { get; private set; }
        public uint Identifier { get; }
        public ISceneViewer Viewer { get; }
        public IComponent<UIComponentData>[] Components => _components.ToArray();

        public IComponent<T> AddComponent<T>() where T : UIComponentData, new() => (IComponent<T>)AddComponent(typeof(T));

        public IComponent<UIComponentData> AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(UIComponentData)))
                throw new ComponentException($"type is not {nameof(UIComponentData)}");
            var component = Component<UIComponentData>.CreateInstance(type);
            InitializeComponent(component);
            if (component.DataTypeIs<RectTransform>())  
            {
                Transform = (IComponent<RectTransform>)component;
                component.CastModify<RectTransform>(transform =>
                {
                    transform.Parent ??= Undefined.Canvas.Transform;
                });
            }
            return component;
        }
        private void InitializeComponent(IComponent<UIComponentData> component)
        {
            component.Modify(data =>
            {
                TargetViewProperty.SetValue(data, this);
            });
            RequireComponent.AddRequirements(component, this);
            _components.Add(component);
        }
        public IComponent<UIComponentData>[] AddComponents(params Type[] types)
        {
            return types.Select(AddComponent).ToArray();
        }

        public void Close()
        {
            OnClose.Invoke(new UICloseEventData(this, Viewer));
            Destroy();
        }


        public bool ContainsViewer(ISceneViewer viewer) => Viewer == viewer;

        public bool ContainsComponent<T>() where T : UIComponentData
        {
            return GetComponent(typeof(T)) != null;
        }

        public bool ContainsComponent(Type type)
        {
            return GetComponent(type) != null;
        }

        public IComponent<T> GetComponent<T>() where T : UIComponentData => _components.FirstOrDefault(c => c.DataTypeIs(typeof(T))) as IComponent<T>;

        public bool TryGetComponent<T1>(out IComponent<T1> component) where T1 : UIComponentData
        {
            throw new NotImplementedException();
        }

        public IComponent<UIComponentData> GetComponent(Type type) => _components.FirstOrDefault(c => c.DataTypeIs(type));

        protected override void DoDestroy()
        {
            Transform.Read(t =>
            {
                for (var i = 0; i < t.Childs.Count; i++) t.Childs[i].TargetObject.Destroy();
            });
        }
    }
}