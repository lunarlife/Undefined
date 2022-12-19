using System.Collections.Generic;
using Events.Tick;
using GameEngine.Exceptions;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Core;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Input;
using UndefinedNetworking.GameEngine.UI.Components;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using UnityEngine;
using Utils.Dots;
using Utils.Events;
using Color = Utils.Color;
using Rect = Utils.Rect;

namespace GameEngine.UI.Windows
{
    public abstract class Window : UIElement
    {
        private static List<UIView> _floatingWindows = new();
        private static UIView _targetWindow;
        private static Dot2Int? _movePos;
        private List<UIElement> _elements = new();
        private Panel _contentPanel;
        private RectMask _mask;
        private Panel _menuPanel;
        private Rect _menuRect;
        private readonly List<UIComponent> _components = new();
        public IReadOnlyList<UIElement> Elements => _elements;

        static Window()
        {
            EventManager.RegisterStaticEvents<Window>();
        }
        
        public Window(Rect rect, Sprite menuSprite = null, int layer = 0, Color? menuColor = null, Sprite windowSprite = null, Color? windowColor = null, string name = "", UIBind? bind = null, Side pivot = Side.Center, Window parent = null, bool isActive = true) 
        {
            _menuRect = new Rect(rect.Position.X, rect.Position.Y + rect.Height, rect.Width, MenuYSize);
            _menuPanel = new Panel(new Rect(0,rect.Height, rect.Width, MenuYSize), parent: this, color: menuColor, sprite: menuSprite, isActive: true);
            _mask = new RectMask(new Rect(0, MenuYSize, rect.Width, rect.Height), Margins.Zero, bind: new UIBind
            {
                Side = Side.TopLeft
            }, parent: _menuPanel);
            _contentPanel = new Panel(new Rect(0,0, rect.Width, rect.Height), parent: _mask, sprite: windowSprite, color: windowColor);
            //if (this is IFloating) _floatingWindows.Add(this);
        }

        private const int MenuYSize = 30;

        public void AddElement(UIElement element)
        {
            /*if (_elements.Contains(element))
                throw new WindowException("Element is already added");
            if (element.Transform.Parent?.TargetView is not Canvas)
                throw new WindowException("Element has parent");
            element.Transform.Parent = _contentPanel.Transform;
            _elements.Add(element);*/
        }

        public void Remove(UIElement element)
        {
            if (!_elements.Contains(element))
                throw new WindowException("Element not found");
        }
        [EventHandler]
        private void OnAsyncTick(AsyncTickEvent e)
        {
            var mousePos = Undefined.MouseScreenPosition;
            /*if (this is IScrollable scrollable)
            {
                var contentRect = _contentPanel.Transform.OriginalRect;
                if (contentRect.Width != scrollable.ScrollSize.X || contentRect.Height != scrollable.ScrollSize.Y)
                    _contentPanel.Transform.OriginalRect = new Rect(contentRect.Position, scrollable.ScrollSize);
                if (Transform.OriginalRect.DotInRect(mousePos))
                {
                    var move = Dot2.Zero;
                    switch (this)
                    {
                        case IHorizontalScrollable { CanHorizontalScrollNow: true } hScrollable
                            when Settings.Binds.ScrollHorizontal.IsPressed():
                            move.X = Engine.MouseScroll * hScrollable.HorizontalScrollSpeed;
                            break;
                        case IVerticalScrollable { CanVertialScrollNow: true } vScrollable:
                            move.Y = Engine.MouseScroll * vScrollable.VerticalScrollSpeed;
                            break;
                    }

                    var currentRect = Transform.OriginalRect;
                    var scrollSize = scrollable.ScrollSize;
                    var contentPosition = contentRect.Position;
                    var pos = new Dot2(
                        MathUtils.ClampOut(contentPosition.X + move.X, -scrollSize.X + currentRect.Width, 0),
                        MathUtils.ClampOut(contentPosition.Y + move.Y, -scrollSize.Y + currentRect.Height, 0));
                    _contentPanel.Transform.OriginalRect = new Rect(pos, scrollSize.X, scrollSize.Y);
                }
            }    */
            /*if(_targetWindow is null) return;
            var menuRect = _targetWindow._menuRect;
            var transform = _targetWindow.Transform;
            var currentDelta = new Dot2Int(menuRect.Position.X + menuRect.Width - mousePos.X,
                menuRect.Position.Y - mousePos.Y);
            if (_movePos is not { } prDelta)
                _movePos = prDelta = currentDelta;
            var cPos = transform.OriginalRect.Position;
            var rect = new Rect(cPos.X - (currentDelta.X - prDelta.X),
                cPos.Y - (currentDelta.Y - prDelta.Y) + transform.OriginalRect.Height, menuRect.Width,
                MenuYSize);
            var pressed = Settings.Binds.WindowInsertRectChange.IsPressed();
            var heightOffset = pressed ? transform.OriginalRect.Height : 0;
            var canvasRect = Undefined.Canvas.Transform.AnchoredRect;
            rect = new Rect(
                rect.Insert(new Rect(canvasRect.Position.X, canvasRect.Position.Y + heightOffset, canvasRect.Width,
                    canvasRect.Height - heightOffset)),
                rect.Width, rect.Height);
            transform.OriginalRect =
                new Rect(new Dot2Int(rect.Position.X, rect.Position.Y - transform.OriginalRect.Height),
                    transform.OriginalRect.Width, transform.OriginalRect.Height);
            _targetWindow._menuRect = rect;*/

        }
        [EventHandler]
        private static void OnTick(TickEvent e)
        {
            var mousePos = Undefined.MouseScreenPosition;

            if (!Undefined.IsPressed(MouseKey.Left))
            {
                _targetWindow = null;
                _movePos = null;
            }
            if (_targetWindow is null)
                for (var i = 0; i < _floatingWindows.Count; i++)
                {
                    var window = _floatingWindows[i];
                    //if (!((IFloating)window).IsCanFloatingNow) continue;
                    if (!Undefined.IsPressed(MouseKey.Left, ClickState.Down)) continue;
                    /*if (window._menuRect.DotInRect(mousePos))
                    {
                        _targetWindow = window;
                        break;                            
                    }*/
                    _targetWindow = null;
                }
            
        }
    }
}