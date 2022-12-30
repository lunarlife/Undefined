using System.Linq;
using GameData;
using GameEngine.GameObjects.Core;
using GameEngine.Resources;
using UECS;
using UndefinedNetworking.GameEngine.UI;
using UndefinedNetworking.GameEngine.UI.Components;
using UndefinedNetworking.GameEngine.UI.Components.RectMask;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Dots;
using Rect = Utils.Rect;
using RectTransform = UndefinedNetworking.GameEngine.UI.Components.RectTransform;
using Shader = UnityEngine.Shader;

namespace GameEngine.UI.Systems
{
    public class ScrollSystem : ISyncSystem
    {


        [AutoInject] private Filter<ScrollComponent> _scrolls;
        [ChangeHandler] private Filter<ScrollComponent> _changedScrolls;

        public void Init()
        {
        }

        public void Update()
        {
            
            foreach (var result in _scrolls)
            {
                var component = result.Get1();
                var view = component.TargetView;
                var transform = view.Transform;

                if (transform.Childs.FirstOrDefault(ch => ch.TargetView is UIView)?.TargetView is not { } content)
                {
                    content = view.Viewer.ActiveScene.OpenView(new ViewParameters
                    {
                        Parent = transform,
                        OriginalRect = new Rect(0,0, transform.OriginalRect.WidthHeight),
                        Margins = transform.Margins
                    });
                    content.Transform.Update();
                }
                for (var index = 0; index < transform.Childs.Count; index++)
                {
                    var tr = transform.Childs[index];
                    if(tr.TargetView is UIView) continue;
                    if (((ObjectCore)tr.TargetView).LocalParent != null) continue;
                    ((ObjectCore)tr.TargetView).LocalParent = (ObjectCore)content;
                    tr.TargetView.Transform.Update();
                }
                if(!transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) continue;
                var move = new Dot2(Undefined.MouseScroll * component.HorizontalScrollSpeed, Undefined.MouseScroll * component.VerticalScrollSpeed);
                var contentPosition = content.Transform.OriginalRect.Position;
                var currentRect = content.Transform.OriginalRect;
                var viewRect = component.ViewRect;
                var pos = new Dot2(
                    MathUtils.ClampOut(contentPosition.X - move.X, -viewRect.Position.X,
                        transform.AnchoredRect.Width - (viewRect.Position.X + viewRect.Width)),
                    MathUtils.ClampOut(contentPosition.Y - move.Y, -viewRect.Position.Y,
                        transform.AnchoredRect.Height - (viewRect.Position.Y + viewRect.Height)));
                content.Transform.OriginalRect = new Rect(pos, currentRect.Width, currentRect.Height);
                
                /*
                var component = result.Get1();
                var view = component.TargetView;
                var transform = view.Transform;
                if (((ObjectCore)view).LocalParent is not IUIView mask)
                {
                    mask = view.Viewer.ActiveScene.OpenView(new ViewParameters
                    {
                        Parent = transform.Parent,
                        OriginalRect = transform.OriginalRect,
                        Bind = transform.Bind,
                        Layer = transform.Layer,
                        Margins = transform.Margins,
                        Pivot = transform.Pivot,
                        IsActive = transform.IsActive
                    });
                    ((ObjectCore)view).LocalParent = (ObjectCore)mask;
                    transform.Update();
                }
                var contentRect = transform.AnchoredRect;
                var contentSize = component.ContentSize;
                if(!new Rect(contentSize.Position.X + contentRect.Position.X, contentSize.Position.Y + contentSize.Height - (contentRect.Position.Y + contentRect.Height),
                       contentSize.Width - (contentRect.Position.X + contentRect.Width), contentRect.Position.Y).DotInRect(Undefined.MouseScreenPosition)) continue;
                var currentRect = transform.OriginalRect;
                var move = Dot2.Zero;
                move.Y = Undefined.MouseScroll * component.VerticalScrollSpeed;
                move.X = Undefined.MouseScroll * component.HorizontalScrollSpeed;
                var contentPosition = transform.OriginalRect.Position;
                var pos = new Dot2(
                    MathUtils.ClampOut(contentPosition.X - move.X, transform.AnchoredRect.Width - currentRect.Width,
                        component.ViewRect.Position.X),
                    MathUtils.ClampOut(contentPosition.Y - move.Y, transform.AnchoredRect.Height - currentRect.Height - component.ViewRect.Height + component.ViewRect.Position.Y,
                        component.ViewRect.Position.Y));
                transform.OriginalRect = new Rect(pos, currentRect.Width, currentRect.Height);*/
            }
            foreach (var scroll in _changedScrolls)
            {
                var component = scroll.Get1();
                var transform = component.TargetView.Transform;
                var aRect = transform.AnchoredRect;
                var mask = ((ObjectCore)component.TargetView).GetOrAddUnityComponent<RectMask2D>();
                mask.padding = new Vector4(component.ViewRect.Position.X, component.ViewRect.Position.Y,
                    aRect.Width - (component.ViewRect.Position.X + component.ViewRect.Width), aRect.Height -
                    (component.ViewRect.Position.Y + component.ViewRect.Height));
            }
        }
    }
}