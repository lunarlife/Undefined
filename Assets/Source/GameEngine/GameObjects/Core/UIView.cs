using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Events.UI;
using GameEngine.GameObjects.Core.Unity;
using Networking;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Components;
using Utils.Events;

namespace GameEngine.GameObjects.Core
{
    public sealed class UIView : ObjectCore, IUIView, IEventCaller<UICloseEvent>
    {
        private static readonly PropertyInfo ComponentInfo = typeof(UIUnityComponentAdapter).GetProperty("Component", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        private static readonly PropertyInfo TargetViewProperty = typeof(UIComponent).GetProperty("TargetView", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        private readonly List<UIComponent> _components = new();
        private readonly RectTransform _transform;
        private readonly UnityEngine.RectTransform _unityTransform;
        private readonly List<UIView> _childs;


        public IRectTransform Transform => _transform;
        public Identifier Identifier { get; }
        public IUIViewer Viewer { get; }
        public IUIElement TargetElement { get; }
        public IEnumerable<Component> Components => _components;


        private UIView(UIElement element, IUIViewer viewer, UIView parent)
        {
            Identifier = new Identifier();
            Viewer = viewer;
            TargetElement = element;
            var parameters = element.CreateNewView(viewer);
            _unityTransform = (Instance as UnityUIObject)!.Transform;
            _transform = new RectTransform(_unityTransform, this, parent is null ? element is Canvas or Camera ? null : Undefined.Canvas.Transform : parent.Transform,
                parameters.IsActive,
                parameters.Layer, parameters.Margins, parameters.OriginalRect, parameters.Pivot, parameters.Bind);
            _childs = element.Childs.Select(ch => new UIView(ch as UIElement, viewer, this)).ToList();
            element.OnCreateView(this);
        }

        public UIView(UIElement element, IUIViewer viewer) : this(element, viewer, null)
        {
            
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
            var component = (UIComponent)Activator.CreateInstance(type);
            Undefined.Systems.Add(component);
            InitializeComponent(component);
            return component;
        }

        public UIComponent[] AddComponents(params Type[] types) => types.Select(AddComponent).ToArray();

        private void InitializeComponent(UIComponent component)
        {
            TargetViewProperty.SetValue(component, this);
            if (component is UIUnityComponentAdapter adapter)
            {
                Undefined.CallSynchronously(() =>
                {
                    var unity = AddUnityComponent(adapter.ComponentType);
                    ComponentInfo.SetValue(component, unity);
                    component.Update();
                });
            }
            else
            {
                component.Update();
            }
            _components.Add(component);
        }
        public void Close()
        {
            Viewer.Close(this);
        }

        public bool ContainsComponent<T>() where T : UIComponent => GetComponentOfType(typeof(T)) != null;

        public bool ContainsComponent(Type type) => GetComponentOfType(type) != null;

        public T GetComponentOfType<T>() where T : UIComponent => _components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;

        public UIComponent GetComponentOfType(Type type) => _components.FirstOrDefault(c => c.GetType() == type);
        public static implicit operator UnityEngine.RectTransform(UIView view) => view._unityTransform;


    }
}