using GameEngine.GameObjects.Core;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using UnityEngine.UI;
using Rect = Utils.Rect;

namespace GameEngine.GameObjects
{
    //TODO: update to new system
    public class RectMask : UIElement
    {
        private Margins _margins;
        private RectMask2D _mask;
        
        public Margins Margins
        {
            get => _margins;
            set
            {
                _margins = value;
                if(_mask is not null)
                    UpdateMarginds();
            }
        }

        public RectMask(Rect rect, Margins margins, string name = "", UIBind? bind = null, Side pivot = Side.Center, int layer = 0, UIElement parent = null, bool isActive = true) 
        {/*
            if (rect.Width < margins.Left)
                throw new MarginsException($"{nameof(margins.Left)} value: ({margins.Left}) must be less than {nameof(rect.Width)} value: ({rect.Width})");
            if (rect.Width < margins.Right)
                throw new MarginsException($"{nameof(margins.Right)} value: ({margins.Right}) must be less than {nameof(rect.Width)} value: ({rect.Width})");
            if (rect.Height < margins.Top)
                throw new MarginsException($"{nameof(margins.Top)} value: ({margins.Top}) must be less than {nameof(rect.Height)} value: ({rect.Height})");
            if (rect.Height < margins.Bottom)
                throw new MarginsException($"{nameof(margins.Bottom)} value: ({margins.Bottom}) must be less than {nameof(rect.Height)} value: ({rect.Height})");*/
            _margins = margins;
            /*AddUnityComponent<RectMask2D>(mask =>
            {
                _mask = mask;
                UpdateMarginds();
            });*/
        }

        private void UpdateMarginds()
        {
            _mask.padding = _margins.ToUnityPadding();
        }

        public override ViewParameters CreateNewView(IUIViewer viewer)
        {
            throw new System.NotImplementedException();
        }
    }
}