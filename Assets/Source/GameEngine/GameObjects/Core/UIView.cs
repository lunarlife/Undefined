using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameEngine.GameObjects.Core.Unity;
using Networking;
using UndefinedNetworking.Events.UIEvents;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Scenes;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Components;
using Utils;
using Utils.Events;

namespace GameEngine.GameObjects.Core
{
    public sealed class UIView : ObjectCore, IUIView, IEventCaller<UICloseEvent>
    {
        private static readonly PropertyInfo ComponentInfo = typeof(UIUnityComponent).GetProperty("Component", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly PropertyInfo TargetViewProperty = typeof(UIComponent).GetProperty("TargetView", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        private static readonly MethodInfo InitializeVoid = ReflectionUtils.GetMethod(typeof(Component), "Initialize")!;
        private readonly List<UIComponent> _components = new();
        private readonly UnityEngine.RectTransform _unityTransform;
        private readonly List<UIView> _childs;


        public RectTransform Transform { get; private set; }
        public Identifier Identifier { get; }
        public ISceneViewer Viewer { get; }
        public IEnumerable<UIComponent> Components => _components;

        public UIView(ISceneViewer viewer, ViewParameters parameters)
        {
            Identifier = new Identifier();
            Viewer = viewer;
            _unityTransform = (Instance as UnityUIObject)!.Transform;
            var rectTransform = AddComponent<RectTransform>();
            rectTransform.ApplyParameters(parameters);
            if(rectTransform.Parent is null && Undefined.Canvas?.Transform is { } canvas)
                rectTransform.Parent = canvas;
            rectTransform.Update();
            Transform = rectTransform;
        }
        protected override void DoDestroy()
        {
            for (var i = 0; i < Transform.Childs.Count; i++)
            {
                Transform.Childs[i].TargetView.Destroy();
            }
        }
 
        public T AddComponent<T>() where T : UIComponent, new() => AddComponent(typeof(T)) as T;
        public UIComponent AddComponent(Type type)
        {
            if (!type.IsSubclassOf(typeof(UIComponent))) throw new ComponentException($"type is not {nameof(UIComponent)}");
            var ctor = ReflectionUtils.GetConstructor(type);
            if (ctor == null) throw new ComponentException("component has no empty constructor");
            var component = (ctor.Invoke(Array.Empty<object>()) as UIComponent)!;
            InitializeComponent(component);
            return component;
        }

        public UIComponent[] AddComponents(params Type[] types) => types.Select(AddComponent).ToArray();

        private void InitializeComponent(UIComponent component)
        {
            TargetViewProperty.SetValue(component, this);
            if (component is UIUnityComponent adapter)
            {
                Undefined.CallSynchronously(() =>
                {
                    var unity = AddUnityComponent(adapter.ComponentType);
                    ComponentInfo.SetValue(component, unity);
                });
            }
            RequireComponent.AddRequirements(component, this);
            InitializeVoid.Invoke(component, Array.Empty<object>());
            _components.Add(component);
        }

        public void Close()
        {
            Destroy();
        }

        public bool ContainsComponent<T>() where T : UIComponent => GetComponent(typeof(T)) != null;

        public bool ContainsComponent(Type type) => GetComponent(type) != null;

        public T GetComponent<T>() where T : UIComponent => _components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;

        public UIComponent GetComponent(Type type) => _components.FirstOrDefault(c => c.GetType() == type);
        public static implicit operator UnityEngine.RectTransform(UIView view) => view._unityTransform;


    }
}