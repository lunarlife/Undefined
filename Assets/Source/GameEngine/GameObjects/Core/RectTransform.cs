using System;
using System.Collections.Generic;
using GameEngine.Exceptions;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using UnityEngine;
using Utils.Dots;
using Rect = Utils.Rect;

namespace GameEngine.GameObjects.Core
{
    public class RectTransform : IRectTransform
    {
        private readonly UnityEngine.RectTransform _rectTransform;
        private Rect _originalRect;
        private Rect _anchoredRect;
        private List<RectTransform> _childs = new();
        private RectTransform _parent;
        private Dot2 _pivot;
        private Side _pivotSide;
        private Margins _margins;
        private Dot2Int _bindMultiplier = Dot2Int.Zero;
        private Rect _uiRect;
        private bool _isActive;

        public Rect OriginalRect
        {
            get => _originalRect;
            set
            {
                _originalRect = value;
                UpdateRectAsync(true);
            }
        }

        public Margins Margins
        {
            get => _margins;
            set
            {
                _margins = value;
                UpdateRectAsync(true);
            }
        }

        public Rect AnchoredRect => _anchoredRect;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                Undefined.CallSynchronously(() => _rectTransform.gameObject.SetActive(_isActive));
            }
        }

        public IRectTransform Parent
        {
            get => _parent;
            set
            {
                if (value == this)
                    throw new ObjectException("parent cant be this object");
                if(value is not RectTransform transform)
                    throw new ObjectException($"unknown {nameof(IRectTransform)}");
                _parent?._childs.Remove(this);
                _parent = transform;
                _parent?._childs.Add(this);
                if(_rectTransform is not null)
                    Undefined.CallSynchronously(UpdateParent);
            }
        }
        private void UpdateParent()
        {
            _rectTransform.SetParent(_parent?._rectTransform);
            UpdateRectAsync(true);
        }
        public int Layer { get; set; }
        public IUIView TargetView { get; }

        public IReadOnlyList<IRectTransform> Childs => _childs;

        public Side Pivot
        {
            get => _pivotSide;
            set
            {
                _pivotSide = value;
                UpdateRectAsync(true);
            }
        }

        public UIBind Bind { get; set; }


        private void UpdateRectAsync(bool updateCurrent)
        {
            UpdatePivot();
            if (_parent is not null && Bind.IsExpandable)
            {
                var localPosition = (Dot2)_originalRect.Position;
                localPosition.X += _margins.Left;
                localPosition.Y += _margins.Bottom;
                var wh = (Dot2)_originalRect.WidthHeight;
                wh.X += _margins.Right + _margins.Left;
                wh.Y += _margins.Top + _margins.Bottom;
                switch (Bind.Side)
                {
                    case Side.TopLeft:
                    case Side.TopRight:
                    case Side.RightBottom:
                    case Side.Center:
                    case Side.LeftBottom:
                        wh = (Dot2)_parent._anchoredRect.WidthHeight - wh;
                        break;
                    case Side.Top:
                        localPosition.Y = _parent._anchoredRect.Height - localPosition.Y - wh.Y;
                        wh.X = _parent._anchoredRect.Width - wh.X;
                        break;
                    case Side.Right:
                        localPosition.X = _parent._anchoredRect.Width - localPosition.X - wh.X;
                        wh.Y = _parent._anchoredRect.Height - wh.Y;
                        break;
                    case Side.Bottom:
                        wh.X = _parent._anchoredRect.Width - wh.X;
                        break;
                    case Side.Left:
                        wh.Y = _parent._anchoredRect.Height  - wh.Y;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                var anchoredPosition = (Dot2)_parent.AnchoredRect.Position + localPosition;
                _anchoredRect = new Rect(anchoredPosition, wh);
                _uiRect = new Rect(localPosition + wh * _pivot, wh) ;
            }
            else
            {
                var localPosition = (Dot2)_originalRect.Position;
                localPosition.X += _margins.Left;
                localPosition.Y += _margins.Bottom;
                var wh = new Dot2(_originalRect.Width, _originalRect.Height);
                wh.X -= _margins.Right - _margins.Left;
                wh.Y -= _margins.Top - _margins.Bottom;
                var localRect = new Rect(localPosition, wh);
                var withBind = (Dot2)GetPositionWithBind(localRect);
                var anchoredPosition = _parent is null ? withBind : (Dot2)_parent.AnchoredRect.Position + withBind;
                _anchoredRect = new Rect(anchoredPosition, wh);
                _uiRect = new Rect(withBind + (Dot2)_originalRect.WidthHeight * _pivot, wh);
            }
            for (var i = 0; i < _childs.Count; i++)
            {
                _childs[i].UpdateRectAsync(false);
            }
            Undefined.CallSynchronously(() =>
            {
                if(updateCurrent)
                    UpdateRect();
                for (var i = 0; i < _childs.Count; i++)
                {
                    _childs[i].UpdateRect();
                }
            });
        }

        private Dot2Int GetPositionWithBind(Rect rect) => _parent is null ? rect.Position :
            Bind.Side switch
            {
                Side.TopLeft => new Dot2Int(rect.Position.X, _parent._anchoredRect.Height - rect.Position.Y - rect.Height),
                Side.Top => new Dot2Int(rect.Position.X - rect.Width / 2 + _parent._anchoredRect.Width / 2, _parent._anchoredRect.Height - rect.Position.Y - rect.Height),
                Side.TopRight => new Dot2Int(_parent._anchoredRect.Width - rect.Position.X - rect.Width, _parent._anchoredRect.Height - rect.Position.Y - rect.Height),
                Side.Right => new Dot2Int(_parent._anchoredRect.Width - rect.Position.X - rect.Width, rect.Position.Y - rect.Height / 2 + _parent._anchoredRect.Height / 2),
                Side.RightBottom => new Dot2Int(_parent._anchoredRect.Width - rect.Position.X - rect.Width, rect.Position.Y),
                Side.Bottom => new Dot2Int(rect.Position.X - rect.Width / 2 + _parent._anchoredRect.Width / 2, rect.Position.Y),
                Side.LeftBottom => rect.Position,
                Side.Left => new Dot2Int(rect.Position.X, rect.Position.Y - rect.Height / 2 + _parent._anchoredRect.Height / 2),
                Side.Center => new Dot2Int(rect.Position.X - rect.Width / 2 + _parent._anchoredRect.Width / 2, rect.Position.Y - rect.Height / 2 + _parent._anchoredRect.Height / 2),
                _ => throw new ArgumentOutOfRangeException()
            };

        private void UpdateRect()
        {
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.zero;
            _rectTransform.localScale  = Vector3.one;
            _rectTransform.pivot = _pivot.ToUnityVector();
            _rectTransform.localPosition = new Vector3(0,0, -Layer);
            _rectTransform.anchoredPosition = _uiRect.Position.ToUnityVector();
            _rectTransform.sizeDelta = _uiRect.WidthHeight.ToUnityVector();
        }
        private void UpdatePivot()
        {
            _pivot = _pivotSide switch
            {
                Side.Left => new Dot2(0, .5f),
                Side.Right => new Dot2(1, .5f),
                Side.Bottom => new Dot2(.5f, 0),
                Side.Top => new Dot2(.5f, 1),
                Side.Center => new Dot2(.5f, .5f),
                Side.TopLeft => new Dot2(0, 1),
                Side.TopRight => new Dot2(1, 1),
                Side.RightBottom => new Dot2(1, 0),
                Side.LeftBottom => new Dot2(0, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public RectTransform(UnityEngine.RectTransform rectTransform, IUIView target, IRectTransform parent, bool isActive, int layer, Margins margins, Rect originalRect, Side pivot, UIBind bind)
        {
            _rectTransform = rectTransform;
            _parent = parent as RectTransform;
            TargetView = target;
            _margins = margins;
            _originalRect = originalRect;
            _pivotSide = pivot;
            IsActive = isActive;
            Layer = layer;
            Bind = bind;
            Undefined.CallSynchronously(UpdateParent);
        }
        
    }
}