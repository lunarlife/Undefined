using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.GameObjects.Core.Unity;
using Networking;
using UndefinedNetworking.Events.UIEvents;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using Utils;
using Utils.Events;
using RectTransform = UnityEngine.RectTransform;

namespace GameEngine.GameObjects.Core
{
    public sealed class UIView : ObjectCore, IUIView, IEventCaller<UICloseEvent>
    {
        private static readonly PropertyInfo ComponentInfo = typeof(UIUnityComponent).GetProperty("Component",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        private static readonly PropertyInfo TargetViewProperty = typeof(UIComponent).GetProperty("TargetView",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        private static readonly MethodInfo InitializeVoid = ReflectionUtils.GetMethod(typeof(Component), "Initialize")!;
        private readonly List<UIView> _childs;
        private readonly List<UIComponent> _components = new();
        private readonly RectTransform _unityTransform;

        public UIView(ISceneViewer viewer, ViewParameters parameters)
        {
            Identifier = (uint)(uint.MaxValue - viewer.ActiveScene.Objects.Length);
            Viewer = viewer;
            _unityTransform = (Instance as UnityUIObject)!.Transform;
            var rectTransform = AddComponent<UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform>();
            rectTransform.ApplyParameters(parameters);
            if (rectTransform.Parent is null && Undefined.Canvas?.Transform is { } canvas)
                rectTransform.Parent = canvas;
            rectTransform.Update();
            Transform = rectTransform;
        }


        public UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform Transform { get; }
        public uint Identifier { get; }
        public ISceneViewer Viewer { get; }
        public UIComponent[] Components => _components.ToArray();

        public T AddComponent<T>() where T : UIComponent, new()
        {
            return AddComponent(typeof(T)) as T;
        }

        public UIComponent AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(UIComponent)))
                throw new ComponentException($"type is not {nameof(UIComponent)}");
            var ctor = ReflectionUtils.GetConstructor(type);
            if (ctor == null) throw new ComponentException("component has no empty constructor");
            var component = (ctor.Invoke(Array.Empty<object>()) as UIComponent)!;
            InitializeComponent(component);
            return component;
        }

        public UIComponent[] AddComponents(params Type[] types)
        {
            return types.Select(AddComponent).ToArray();
        }

        public void Close()
        {
            Destroy();
        }

        public bool ContainsComponent<T>() where T : UIComponent
        {
            return GetComponent(typeof(T)) != null;
        }

        public bool ContainsComponent(Type type)
        {
            return GetComponent(type) != null;
        }

        public T GetComponent<T>() where T : UIComponent
        {
            return _components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;
        }

        public UIComponent GetComponent(Type type)
        {
            return _components.FirstOrDefault(c => c.GetType() == type);
        }

        protected override void DoDestroy()
        {
            for (var i = 0; i < Transform.Childs.Count; i++) Transform.Childs[i].TargetView.Destroy();
        }

        private void InitializeComponent(UIComponent component)
        {
            TargetViewProperty.SetValue(component, this);
            if (component is UIUnityComponent adapter)
                Undefined.CallSynchronously(() =>
                {
                    var unity = AddUnityComponent(adapter.ComponentType);
                    ComponentInfo.SetValue(component, unity);
                });
            RequireComponent.AddRequirements(component, this);
            InitializeVoid.Invoke(component, Array.Empty<object>());
            _components.Add(component);
        }

        public static implicit operator RectTransform(UIView view)
        {
            return view._unityTransform;
        }
    }
}