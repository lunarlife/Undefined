using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.GameObjects.Core.Unity;
using Networking;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using Utils;
using RectTransform = UnityEngine.RectTransform;

namespace GameEngine.GameObjects.Core
{
    public sealed class NetworkUIView : ObjectCore, IUIView
    {
        private static readonly PropertyInfo TargetViewProperty = typeof(UIComponent).GetProperty("TargetView",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        private static readonly MethodInfo InitializeVoid = ReflectionUtils.GetMethod(typeof(Component), "Initialize")!;
        private readonly List<NetworkUIView> _childs;
        private readonly List<UIComponent> _components = new();
        private readonly RectTransform _unityTransform;


        public NetworkUIView(ISceneViewer viewer, uint identifier)
        {
            Identifier = identifier;
            Viewer = viewer;
            _unityTransform = (Instance as UnityUIObject)!.Transform;
        }


        public UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform Transform { get; private set; }
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
            var component = (Activator.CreateInstance(type) as UIComponent)!;
            InitializeComponent(component);
            if (component is UndefinedNetworking.GameEngine.Scenes.UI.Components.RectTransform transform)
            {
                transform.Parent ??= Undefined.Canvas.Transform;
                Transform = transform;
            }

            component.Update();
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
            RequireComponent.AddRequirements(component, this);
            InitializeVoid.Invoke(component, Array.Empty<object>());
            _components.Add(component);
        }

        public static implicit operator RectTransform(NetworkUIView view)
        {
            return view._unityTransform;
        }
    }
}