using System.Linq;
using GameEngine.GameObjects.Core;
using UECS;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Utils.Dots;
using Rect = Utils.Rect;

namespace GameEngine.UI.Systems
{
    public class ScrollSystem : ISyncSystem
    {
        [ChangeHandler] private Filter<IComponent<Scroll>> _changedScrolls;


        [AutoInject] private Filter<IComponent<Scroll>> _scrolls;

        public void Init()
        {
        }

        public void Update()
        {
            foreach (var result in _scrolls)
            {
                result.Get1().Read(component =>
                {
                    var view = component.TargetObject;
                    view.Transform.Read(transform =>
                    {


                        if (transform.Childs.FirstOrDefault(ch => ch.TargetObject is UIView)?.TargetObject is not
                            { } content)
                        {
                            content = Undefined.Player.ActiveScene.OpenView(new ViewParameters
                            {
                                Parent = view.Transform,
                                OriginalRect = new Rect(0, 0, transform.OriginalRect.WidthHeight),
                                Margins = transform.Margins
                            });
                            content.Transform.Update();
                        }

                        for (var index = 0; index < transform.Childs.Count; index++)
                        {
                            var tr = transform.Childs[index];
                            if (tr.TargetObject is UIView) continue;
                            if (((ObjectCore)tr.TargetObject).LocalParent != null) continue;
                            ((ObjectCore)tr.TargetObject).LocalParent = (ObjectCore)content;
                            tr.TargetObject.Transform.Update();
                        }

                        if (!transform.AnchoredRect.DotInRect(Undefined.MouseScreenPosition)) return;
                        var move = new Dot2(Undefined.MouseScroll * component.HorizontalScrollSpeed,
                            Undefined.MouseScroll * component.VerticalScrollSpeed);
                        content.Transform.Modify(contentTransform =>
                        {
                            var contentPosition = contentTransform.OriginalRect.Position;
                            var currentRect = contentTransform.OriginalRect;
                            var viewRect = component.ViewRect;
                            var pos = new Dot2(
                                MathUtils.ClampOut(contentPosition.X - move.X, -viewRect.Position.X,
                                    transform.AnchoredRect.Width - (viewRect.Position.X + viewRect.Width)),
                                MathUtils.ClampOut(contentPosition.Y - move.Y, -viewRect.Position.Y,
                                    transform.AnchoredRect.Height - (viewRect.Position.Y + viewRect.Height)));
                            contentTransform.OriginalRect = new Rect(pos, currentRect.Width, currentRect.Height);
                        });
                       
                    });
                });

            }

            foreach (var scroll in _changedScrolls)
            {
                scroll.Get1().Read(component =>
                {
                    component.TargetObject.Transform.Read(transform =>
                    {
                        var aRect = transform.AnchoredRect;
                        var mask = ((ObjectCore)component.TargetObject).GetOrAddUnityComponent<RectMask2D>();
                        mask.padding = new Vector4(component.ViewRect.Position.X, component.ViewRect.Position.Y,
                            aRect.Width - (component.ViewRect.Position.X + component.ViewRect.Width), aRect.Height -
                            (component.ViewRect.Position.Y + component.ViewRect.Height));
                    });
                });
            }
        }
    }
}