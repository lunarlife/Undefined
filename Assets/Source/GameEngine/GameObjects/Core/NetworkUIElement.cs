using System;
using System.Collections.Generic;
using Networking;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Components;

namespace GameEngine.GameObjects.Core
{
    public class NetworkUIElement : IUIElement, IDisposable
    {
        private ViewParameters? _parameters;
        private UINetworkComponent[] _components;
        private Identifier _identifier;
        private List<IUIElement> _childs = new();

        public NetworkUIElement(ViewParameters parameters, Identifier identifier, UINetworkComponent[] components = null, NetworkUIElement parent = null)
        {
            Parent = parent;
            _parameters = parameters;
            _components = components;
            _identifier = identifier;
            parent?._childs.Add(this);
        }
            
        public ViewParameters CreateNewView(IUIViewer viewer) => _parameters!.Value;

        public void OnCreateView(IUIView view)
        {
            if (_components is not null && _components.Length != 0)
            {
                var v = (NetworkUIView)view;
                foreach (var component in _components) v.AddNetworkComponent(component);
            }
            Dispose();
        }

        public IUIElement Parent { get; }

        public IEnumerable<IUIElement> Childs => _childs;

        public Identifier Identifier
        {
            get => _identifier;
            set => _identifier = value;
        }

        public void Dispose()
        {
            _parameters = null;
            _components = null;
            _identifier = null;
        }
    }
}