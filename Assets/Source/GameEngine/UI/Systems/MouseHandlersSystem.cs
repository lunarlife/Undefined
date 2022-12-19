using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.Events.Mouse;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.UI.Components.Mouse;
using UndefinedNetworking.Gameplay.Components;
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
        
        public void Init()
        {
            
        }

        public void Update()
        {
            //if(Undefined.IsPressedAny(MouseKey.Left | MouseKey.Middle | MouseKey.Right,ClickState.Down))
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
                        Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
                        Undefined.Logger.Info("send unholding");
                    }
                    continue;
                }
                component.CallEvent(new UIMouseHoldingEvent(component.TargetView));
                if (pHolding || component.TargetView is not NetworkUIView) continue;
                Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
                Undefined.Logger.Info("send holding");
            }
            foreach (var result in _upHandlers)
            {
                var component = result.Get1();
                if (!Undefined.IsPressed(component.Keys, ClickState.Up)) continue;
                if (!component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
                component.CallEvent(new UIMouseUpEvent(component.TargetView));
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
            }  
            foreach (var result in _downHandlers)
            {
                var component = result.Get1();
                if (!Undefined.IsPressed(component.Keys, ClickState.Down)) continue;
                if (!component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
                component.CallEvent(new UIMouseDownEvent(component.TargetView));
                if(component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
            }
            foreach (var result in _enterHandlers)
            {
                var component = result.Get1();
                var inRect = component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition);
                var entered = component.IsEntered;
                component.IsEntered = inRect;
                if (!entered && inRect && component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
            }
            foreach (var result in _exitHandlers)
            {
                var component = result.Get1();
                var outRect = !component.TargetView.Transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition);
                var exited = component.IsExited;
                component.IsExited = outRect;
                if (!exited && outRect && component.TargetView is NetworkUIView)
                    Undefined.SendPackets(new ComponentUpdatePacket(component, component.TargetView.Identifier));
            }
        }
    }
}