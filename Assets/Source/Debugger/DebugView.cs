using System.Collections.Generic;
using System.Linq;
using Events.Tick;
using GameEngine;
using GameEngine.GameObjects;
using GameEngine.GameSettings;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.UI.Elements.Enums;
using UndefinedNetworking.GameEngine.UI.Elements.Structs;
using Utils.Events;
using Rect = Utils.Rect;

namespace Debugger
{
    public class DebugView
    {
        private readonly List<int> _fpsList = new();
        private int _minFps = int.MaxValue, _maxFps = 0;
        private uint _dropCount, _lowDropCount;
        private float _counter = 0;
        private float _cpuDelay;
 
        private Text _text;
        private string _fpstext = "60";
        public static bool AllowCoordinates = true;
        public DebugView()
        {
            ULogger.ShowInfo(Undefined.UIScale.ToString());
            _text = new Text(bind: new UIBind
                {
                    Side = Side.TopRight,
                }, rect: new Rect(5,5,400,600),
                size: new FontSize
            {
                MaxSize = 22
            }, wrapping: new TextWrapping
            {
                Alignment = TextAlignment.TopRight,
                Overflow = TextOverflow.Overflow,
                IsWrapping = true
            }, name:"debug info");
            
            this.RegisterListener();
        }

        [EventHandler]
        private void OnTick(AsyncTickEvent e)
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
        private void OnAsyncFixedTick(AsyncFixedTickEvent e)
        {
            var res = _fpstext;
            res += $"\nCPU: {_cpuDelay}";
            res += $"\nDrops 30fps: {_dropCount} / 10fps: {_lowDropCount}";
            res += $"\nResolution: {Settings.ResolutionUnscaled}";
            if (AllowCoordinates)
            {
                //res += $"\nCamera Position:\t\n{Undefined.Camera.Transform.Position}";
                res += $"\nMouse PositionUnscaled: {Undefined.MouseScreenPositionUnscaled}";
                res += $"\nMouse Position:\t\nScreen: {Undefined.MouseScreenPosition}" 
                    + $"\nWorld: {Undefined.MouseWorldPosition}";
            }
            //_text.Content = res;
        }
    }
}