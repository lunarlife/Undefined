using System.Collections.Generic;
using UndefinedNetworking.GameEngine.UI;

namespace GameEngine.GameObjects.Core
{
    public abstract class UIElement : IUIElement
    {
        private readonly List<IUIElement> _childs = new();
        public IUIElement Parent { get; }

        public IEnumerable<IUIElement> Childs => _childs;
        public abstract ViewParameters CreateNewView(IUIViewer viewer);
        public virtual void OnCreateView(IUIView view) { }

        public UIElement(UIElement parent = null)
        {
            Parent = parent;
            parent?._childs.Add(this);
        }
        
        /*
            if (this is IGridable)
            {
                AddComponent<GridLayoutGroup>(g =>
                {
                    _gridComponent = g;
                    Gridables.Add(this);
                });
            }/**/
        /*if (this is IMouseDownHandler) MouseDownHandlers.Add(this);
        if (this is IMouseUpHandler) MouseUpHandlers.Add(this);
        if (this is IMouseEnterHandler) MouseUpHandlers.Add(this);
        if (this is IMouseExitHandler) MouseUpHandlers.Add(this);
        if (this is IMouseHoldingHandler) MouseUpHandlers.Add(this);#1#*/

        /*
[EventHandler]
private static void OnTick(TickEvent e)
{
    if(Undefined.IsPressedAny(MouseKey.Left | MouseKey.Middle | MouseKey.Right,ClickState.Down))
        for (var i = 0; i < MouseDownHandlers.Count; i++)
        {
            var element = MouseDownHandlers[i]!;
            var downHandler = (element as IMouseDownHandler)!;
            if(!Undefined.IsPressed(downHandler.HandleDownClicks, downHandler.HandleDownClickStates)) continue;
            if(!element._transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
            downHandler.CallEvent(new UIMouseDownEvent(element));
        }
    for (var i = 0; i < MouseUpHandlers.Count; i++)
    {
        var element = MouseDownHandlers[i]!;
        var downHandler = (element as IMouseDownHandler)!;
        if(!Undefined.IsPressed(downHandler.HandleDownClicks, downHandler.HandleDownClickStates)) continue;
        if(!element._transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
        downHandler.CallEvent(new UIMouseDownEvent(element));
    }
}*/


    }
}