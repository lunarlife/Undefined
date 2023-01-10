using System.Collections.Generic;
using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.Events.Mouse;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.Scenes.UI.Components.Mouse;
using UndefinedNetworking.Packets.Components;
using Utils.Events;

namespace GameEngine.UI.Systems
{
    public class MouseHandlersSystem : ISyncSystem
    {
        [AutoInject] private Filter<IComponent<MouseDownHandler>> _downHandlers;

        private readonly HashSet<IComponent<MouseEnterHandler>> _enteredComponents = new();
        private readonly HashSet<IComponent<MouseExitHandler>> _exitedComponents = new();
        [AutoInject] private Filter<IComponent<MouseEnterHandler>> _enterHandlers;
        [AutoInject] private Filter<IComponent<MouseExitHandler>> _exitHandlers;
        [AutoInject] private Filter<IComponent<MouseHoldingHandler>> _holdingHandlers;
        [AutoInject] private Filter<IComponent<MouseUpHandler>> _upHandlers;


        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _holdingHandlers)
            {
                var res = result.Get1();
                res.Read(component =>
                {
                    var holding = false;
                    component.TargetView.Transform.Read(transform => holding = transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition) &&
                                                                               Undefined.IsPressed(component.Keys));
                    var pHolding = component.IsHolding;
                    component.IsHolding = holding;
                    if (!holding)
                    {
                        if (pHolding && component.TargetView is NetworkUIView)
                            Undefined.SendPackets(new UIComponentUpdatePacket(res));
                        return;
                    }

                    component.Event.Invoke(new MouseHoldingEventData(component.TargetView));
                    if (pHolding || component.TargetView is not NetworkUIView) return;
                    Undefined.SendPackets(new UIComponentUpdatePacket(res));
                });
               
            }

            foreach (var result in _upHandlers)
            {
                var res = result.Get1();
                res.Read(component =>
                {
                    if (!Undefined.IsPressed(component.Keys, ClickState.Up)) return;
                    var inRect = false;
                    component.TargetView.Transform.Read(transform => inRect = transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition));
                    if (!inRect) return;
                    component.Event.Invoke(new MouseUpEventData(component.TargetView));
                    //if (component.TargetView is NetworkUIView)
                      //  Undefined.SendPackets(new UIComponentUpdatePacket(res));
                });
            }

            foreach (var result in _downHandlers)
            {
                var res = result.Get1();
                res.Read(component =>
                {
                    if (!Undefined.IsPressed(component.Keys, ClickState.Down)) return;
                    var inRect = false;
                    component.TargetView.Transform.Read(transform => inRect = transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition));
                    if(!inRect) return;
                    component.Event.Invoke(new MouseDownEventData(component.TargetView));
                    if (component.TargetView is NetworkUIView)
                        Undefined.SendPackets(new UIComponentUpdatePacket(res));
                });
            }

            foreach (var result in _enterHandlers)
            {
                var res = result.Get1();
                res.Read(component =>
                {
                    var inRect = false;
                    component.TargetView.Transform.Read(transform => inRect = transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition));
                    var entered = _enteredComponents.Contains(res);
                    if (entered || !inRect)
                    {
                        if (_enteredComponents.Contains(res)) _enteredComponents.Remove(res);
                        return;
                    }

                    if (!_enteredComponents.Contains(res))
                        _enteredComponents.Add(res);
                    if (component.TargetView is NetworkUIView)
                        Undefined.SendPackets(new UIComponentUpdatePacket(res));
                });
            }

            foreach (var result in _exitHandlers)
            {
                var res = result.Get1();
                res.Read(component =>
                {
                    var outRect = false;
                    component.TargetView.Transform.Read(transform => outRect = !transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition));
                    var exited = _exitedComponents.Contains(res);
                    if (exited || !outRect)
                    {
                        if (_exitedComponents.Contains(res)) _exitedComponents.Remove(res);
                        return;
                    }
                    if (!_exitedComponents.Contains(res))
                        _exitedComponents.Add(res);
                    if (component.TargetView is NetworkUIView)
                        Undefined.SendPackets(new UIComponentUpdatePacket(res));
                });
            }
        }
    }
}