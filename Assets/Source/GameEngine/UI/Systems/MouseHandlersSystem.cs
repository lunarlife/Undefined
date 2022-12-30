using System.Collections.Generic;
using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.Events.Mouse;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.UI.Components.Mouse;
using UndefinedNetworking.Packets.Components;
using Utils.Events;

namespace GameEngine.UI.Systems
{
    public class MouseHandlersSystem : ISyncSystem
    {
        [AutoInject] private Filter<MouseUpHandlerComponent> _upHandlers;
        [AutoInject] private Filter<MouseHoldingHandlerComponent> _holdingHandlers;
        [AutoInject] private Filter<MouseDownHandlerComponent> _downHandlers;
        [AutoInject] private Filter<MouseEnterHandlerComponent> _enterHandlers;
        [AutoInject] private Filter<MouseExitHandlerComponent> _exitHandlers;

        private HashSet<MouseEnterHandlerComponent> _enteredComponents = new();
        private HashSet<MouseExitHandlerComponent> _exitedComponents = new();


        public void Init()
        {
            
        }

        public void Update()
        {
            foreach (var result in _holdingHandlers)
            {
                var component = result.Get1();
                var pHolding = component.IsHolding;
                var holding = component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition) && Undefined.IsPressed(component.Keys);
                component.IsHolding = holding;
                if (!holding)
                {
                    if (pHolding && component.TargetView is NetworkUIView)
                    {
                        Undefined.SendPackets(new UIComponentUpdatePacket(component));
                    }
                    continue;
                }
                component.CallEvent(new UIMouseHoldingEvent(component.TargetView));
                if (pHolding || component.TargetView is not NetworkUIView) continue;
                Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }
            foreach (var result in _upHandlers)
            {
                var component = result.Get1();
                if (!Undefined.IsPressed(component.Keys, ClickState.Up)) continue;
                if (!component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
                component.CallEvent(new UIMouseUpEvent(component.TargetView));
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }  
            foreach (var result in _downHandlers)
            {
                var component = result.Get1();
                if (!Undefined.IsPressed(component.Keys, ClickState.Down)) continue;
                if (!component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
                component.CallEvent(new UIMouseDownEvent(component.TargetView));
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }
            foreach (var result in _enterHandlers)
            {
                var component = result.Get1();
                var inRect = component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition);
                var entered = _enteredComponents.Contains(component);
                if (entered || !inRect)
                {
                    if (_enteredComponents.Contains(component)) _enteredComponents.Remove(component);
                    continue;
                }
                if(!_enteredComponents.Contains(component))
                    _enteredComponents.Add(component);
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }
            foreach (var result in _exitHandlers)
            {
                var component = result.Get1();
                var outRect = !component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition);
                var exited = _exitedComponents.Contains(component);
                if (exited || !outRect)
                {
                    if (_exitedComponents.Contains(component)) _exitedComponents.Remove(component);
                    continue;
                }
                if(!_exitedComponents.Contains(component))
                    _exitedComponents.Add(component);
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new UIComponentUpdatePacket(component));
            }
        }
    }
}