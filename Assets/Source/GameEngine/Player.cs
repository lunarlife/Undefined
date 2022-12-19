using System.Collections.Generic;
using GameEngine.GameObjects.Core;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.UI;
using UnityEngine;

namespace GameEngine
{
    public class Player : MonoBehaviour, IUIViewer
    {
        private readonly List<IUIView> _elements = new();

        public IEnumerable<IUIView> ViewElements => _elements;

        public IUIView Open(IUIElement element)
        {
            IUIView view = null;
            if (element is NetworkUIElement networkUIElement)
                view = new NetworkUIView(networkUIElement, this);
            else if (element is UIElement uiElement)
                view = new UIView(uiElement, this);
            if (view == null) throw new ViewException("unknown element");
            _elements.Add(view);
            return view;
        }

        public void Close(IUIView view)
        {
            _elements.Remove(view);
            view.Destroy();
        }
    }
}