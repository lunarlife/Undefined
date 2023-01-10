using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.GameObjects.Core.Unity;
using UndefinedNetworking.Converters;
using UndefinedNetworking.Events.UIEvents;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using Utils;
using Utils.Events;
using Component = UnityEngine.Component;

namespace GameEngine.GameObjects.Core
{
    public sealed class UIView : ObjectCore, IUIView
    {

        private static readonly PropertyInfo TargetViewProperty = typeof(UIComponentData).GetProperty("TargetView",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        private readonly List<UIView> _childs;
        private readonly List<IComponent<UIComponentData>> _components = new();
        public Event<UICloseEventData> OnClose { get; } = new();
        public UIView(ISceneViewer viewer, ViewParameters parameters)
        {
            Identifier = (uint)(uint.MaxValue - viewer.ActiveScene.Objects.Length);
            Viewer = viewer;
            Transform = AddComponent<RectTransform>();
            Transform.Modify(transform =>
            {
                if (parameters.Parent is null && Undefined.Canvas?.Transform is { } canvas)
                    parameters.Parent = canvas;
                transform.ApplyParameters(parameters);
            });
        }


        public IComponent<RectTransform> Transform { get; }
        public uint Identifier { get; }
        public ISceneViewer Viewer { get; }
        public IComponent<UIComponentData>[] Components => _components.ToArray();

        public IComponent<T> AddComponent<T>() where T : UIComponentData, new()
        {
            return AddComponent(typeof(T)) as IComponent<T>;
        }

        public IComponent<UIComponentData> AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(UIComponentData)))
                throw new ComponentException($"type is not {nameof(UIComponentData)}");
            var component = Component<UIComponentData>.CreateInstance(type);
            InitializeComponent(component);
            return component;
        }

        public IComponent<UIComponentData>[] AddComponents(params Type[] types)
        {
            return types.Select(AddComponent).ToArray();
        }

        public void Close()
        {
            Destroy();
        }

        public bool ContainsComponent<T>() where T : UIComponentData
        {
            return GetComponent(typeof(T)) != null;
        }

        public bool ContainsComponent(Type type)
        {
            return GetComponent(type) != null;
        }

        public IComponent<T> GetComponent<T>() where T : UIComponentData
        {
            return _components.FirstOrDefault(c => c.DataTypeIs(typeof(T))) as IComponent<T>;
        }

        public bool TryGetComponent<T1>(out IComponent<T1> component) where T1 : UIComponentData
        {
            component = (IComponent<T1>)_components.FirstOrDefault(c => c.DataTypeIs(typeof(T1)));
            return component is not null;
        }

        public IComponent<UIComponentData> GetComponent(Type type) => _components.FirstOrDefault(c => c.DataTypeIs(type));

        protected override void DoDestroy()
        {
            
            Transform.Read(transform =>
            {
                for (var i = 0; i < transform.Childs.Count; i++) transform.Childs[i].TargetView.Destroy();
            });
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
    }
}