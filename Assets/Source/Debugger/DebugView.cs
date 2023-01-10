using System.Collections.Generic;
using System.Linq;
using Events.Tick;
using GameEngine;
using GameEngine.GameSettings;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Components;
using UndefinedNetworking.GameEngine.Scenes.UI;
using UndefinedNetworking.GameEngine.Scenes.UI.Components;
using UndefinedNetworking.GameEngine.Scenes.UI.Enums;
using UndefinedNetworking.GameEngine.Scenes.UI.Structs;
using Utils;
using Utils.Events;

namespace Debugger
{
    public class DebugView : IEventListener
    {
        public static bool AllowCoordinates = true;
        private readonly List<int> _fpsList = new();
        private float _counter;
        private float _cpuDelay;
        private uint _dropCount, _lowDropCount;
        private string _fpstext = "60";
        private int _minFps = int.MaxValue, _maxFps;

        private readonly IComponent<Text> _text;

        public DebugView()
        {
            _text = Undefined.Player.ActiveScene.OpenView(new ViewParameters
            {
                Bind = new UIBind
                {
                    Side = Side.TopRight
                },
                OriginalRect = new Rect(5, 5, 400, 600)
            }).AddComponent<Text>().Modify(text =>
            {
                text.Size = new FontSize
                {
                    MaxSize = 22
                };
                text.Color = Color.Black;
                text.Wrapping = new TextWrapping
                {
                    Alignment = TextAlignment.TopRight,
                    Overflow = TextOverflow.Overflow,
                    IsWrapping = true
                };
            });
            EventManager.RegisterEvents(this);
        }

        [EventHandler]
        private void OnTick(SyncTickEventData e)
        {
            _cpuDelay = e.DeltaTime;
            var fps = (int)(1f / e.DeltaTime);
            _fpsList.Add(fps);
            _maxFps = fps > _maxFps ? fps : _maxFps;
            _minFps = fps < _minFps ? fps : _minFps;
            if (fps < 10) _lowDropCount++;
            else if (fps < 30) _dropCount++;

            _counter += e.DeltaTime;
            if (!(_counter > 0.3f)) return;
            var argfps = _fpsList.Sum() / _fpsList.Count;
            _fpstext = $"Fps: {argfps} / {_minFps} / {_maxFps}";

            _counter = 0;
            _maxFps = 0;
            _minFps = int.MaxValue;
            _fpsList.Clear();
        }

        [EventHandler]
        private void OnAsyncFixedTick(AsyncFixedTickEventData e)
        {
            var res = _fpstext;
            res += $"\nCPU: {_cpuDelay}";
            res += $"\nDrops 30/10fps: {_dropCount}/{_lowDropCount}";
            res += $"\nResolution: {Settings.ResolutionUnscaled}";
            if (AllowCoordinates)
            {
                //res += $"\nCamera Position:\t\n{Undefined.Camera.Transform.Position}";
                res += $"\nMouse PositionUnscaled: {Undefined.MouseScreenPositionUnscaled}";
                res += $"\nMouse Position:\t\nScreen: {Undefined.MouseScreenPosition}"
                       + $"\nWorld: {Undefined.MouseWorldPosition}";
            }
            _text.Modify(text =>
            {
                text.Content = res;
            });
        }
    }
}