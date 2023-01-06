using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Events.GameEngine;
using Events.GameEngine.Keyboard;
using Events.Tick;
using GameEngine.Components;
using UndefinedNetworking.Events.GameEngine;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Input;
using UnityEngine;
using Utils.Dots;
using Utils.Events;
using Rect = Utils.Rect;

namespace GameEngine
{
    public class EngineEventsInvoker : MonoBehaviour, IEventCaller<EngineStopEvent>
    {
        private static readonly Thread FixedTickThread = new(AsyncFixedTick) { Name = "AsyncFixedTickThread" };
        private static readonly Thread TickThread = new(AsyncTick) { Name = "AsyncTickThread" };
        private static readonly Thread OneSecondThread = new(AsyncOneSecondTick) { Name = "AsyncOneSecondTickThread" };
        private static bool _isAsyncFixedTickWorking;
        private static bool _isOneSecondTickWorking;
        private static bool _isAsyncTickWorking;
        private static SyncTickEvent? _event;

        private static readonly Queue<ActionCallback> SyncActions = new();

        private static readonly HashSet<KeyboardKey> KeyboardAllPressedKeys = new();
        private static readonly HashSet<KeyboardKey> KeyboardPressedKeys = new();
        private static readonly HashSet<KeyboardKey> KeyboardUpKeys = new();
        private static readonly HashSet<KeyboardKey> KeyboardDownKeys = new();


        private static readonly HashSet<MouseKey> MouseAllPressedKeys = new();
        private static readonly HashSet<MouseKey> MousePressedKeys = new();
        private static readonly HashSet<MouseKey> MouseUpKeys = new();
        private static readonly HashSet<MouseKey> MouseDownKeys = new();

        private static readonly object KeyLock = new();
        private static readonly object MouseLock = new();
        private static readonly object SyncLock = new();

        public static Dot2 MouseScreenPositionUnscaled { get; private set; } = Dot2.Zero;
        public static Dot2 MouseScreenPosition { get; private set; } = Dot2.Zero;
        public static Dot2 MouseDeltaUnscaled { get; private set; } = Dot2.Zero;
        public static Dot2 MouseWorldPosition { get; } = Dot2.Zero;
        public static float MouseScroll { get; private set; }
        public static float UIScale { get; private set; } = 1;

        public static bool IsAsyncFixedTickWorking
        {
            get => _isAsyncFixedTickWorking;
            set
            {
                if (_isAsyncFixedTickWorking == value) return;
                _isAsyncFixedTickWorking = value;
                if (_isAsyncFixedTickWorking) FixedTickThread.Start();
            }
        }

        public static bool IsAsyncTickWorking
        {
            get => _isAsyncTickWorking;
            set
            {
                if (_isAsyncTickWorking == value) return;
                _isAsyncTickWorking = value;
                if (_isAsyncTickWorking) TickThread.Start();
            }
        }

        public static bool IsOneSecondTickWorking
        {
            get => _isOneSecondTickWorking;
            set
            {
                if (_isOneSecondTickWorking == value) return;
                _isOneSecondTickWorking = value;
                if (_isOneSecondTickWorking) OneSecondThread.Start();
            }
        }

        private void Awake()
        {
            LoadAssemblies();
            DontDestroyOnLoad(gameObject);
            IsAsyncTickWorking = true;
            IsAsyncFixedTickWorking = true;
            IsOneSecondTickWorking = true;
            Undefined.Startup();
        }

        private async void Update()
        {
            if (Undefined.Canvas?.GetComponent<CanvasComponent>() is { Component: { } } canvas &&
                Undefined.Camera is not null)
            {
                var canvasComponent = (Canvas)canvas.Component;
                if (!UIScale.Equals(canvasComponent.scaleFactor))
                {
                    UIScale = canvasComponent.scaleFactor;
                    var sizeDelta = canvasComponent.GetComponent<RectTransform>().sizeDelta;
                    Undefined.Canvas.Transform.OriginalRect = new Rect(0, 0, (int)sizeDelta.x, (int)sizeDelta.y);
                }

                var mousePosition = Input.mousePosition;
                MouseScroll = Input.mouseScrollDelta.normalized.y;
                MouseDeltaUnscaled = new Dot2(mousePosition.x - MouseScreenPositionUnscaled.X,
                    mousePosition.y - MouseScreenPositionUnscaled.Y);
                MouseScreenPositionUnscaled = new Dot2(mousePosition.x, mousePosition.y);
                MouseScreenPosition = MouseScreenPositionUnscaled / UIScale;
                //MouseWorldPosition = Undefined.Camera.CameraToWorldPositionSynchronously(MouseScreenPosition);
            }

            lock (SyncLock)
            {
                for (var i = 0; i < SyncActions.Count; i++)
                {
                    var action = SyncActions.Dequeue();
                    action?.Invoke();
                }
            }

            lock (KeyLock)
            {
                KeyboardPressedKeys.Clear();
                KeyboardUpKeys.Clear();
                KeyboardDownKeys.Clear();
                KeyboardAllPressedKeys.Clear();
                MousePressedKeys.Clear();
                MouseUpKeys.Clear();
                MouseDownKeys.Clear();
                MouseAllPressedKeys.Clear();
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (keyCode.ToMouseKey() is { } mouseKey)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            MouseDownKeys.Add(mouseKey);
                            MouseAllPressedKeys.Add(mouseKey);
                        }
                        else if (Input.GetKeyUp(keyCode))
                        {
                            MouseUpKeys.Add(mouseKey);
                            MouseAllPressedKeys.Add(mouseKey);
                        }
                        else if (Input.GetKey(keyCode))
                        {
                            MousePressedKeys.Add(mouseKey);
                            MouseAllPressedKeys.Add(mouseKey);
                        }

                        continue;
                    }

                    if (keyCode.ToKeyboardKey() is { } keyboardKey)
                    {
                        if (Input.GetKeyDown(keyCode))
                        {
                            KeyboardDownKeys.Add(keyboardKey);
                            KeyboardAllPressedKeys.Add(keyboardKey);
                        }
                        else if (Input.GetKeyUp(keyCode))
                        {
                            KeyboardUpKeys.Add(keyboardKey);
                            KeyboardAllPressedKeys.Add(keyboardKey);
                        }
                        else if (Input.GetKey(keyCode))
                        {
                            KeyboardPressedKeys.Add(keyboardKey);
                            KeyboardAllPressedKeys.Add(keyboardKey);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(Input.inputString))
            {
                var input = Input.inputString;
                await Task.Run(() => { EventManager.CallEvent(new KeyboardWriteEvent(input)); });
            }

            var tickEvent = new SyncTickEvent(Time.unscaledDeltaTime);
            EventManager.CallEvent(tickEvent);
            _event = tickEvent;
        }

        private void OnDestroy()
        {
            IsAsyncTickWorking = false;
            IsAsyncFixedTickWorking = false;
            IsOneSecondTickWorking = false;
        }

        private void OnApplicationQuit()
        {
            this.CallEvent(new EngineStopEvent());
        }

        private static void LoadAssemblies()
        {
            AppDomain.CurrentDomain.Load("Utils");
            AppDomain.CurrentDomain.Load("UECS");
            AppDomain.CurrentDomain.Load("Networking");
            AppDomain.CurrentDomain.Load("UndefinedNetworking");
        }

        public static bool IsPressedAll(ClickState states, KeyboardKey[] codes)
        {
            lock (KeyLock)
            {
                var pressed = new List<KeyboardKey>();
                if (states == ClickState.Pressed)
                    pressed.AddRange(codes.Where(c => !pressed.Contains(c) && KeyboardPressedKeys.Contains(c)));
                if (pressed.Count != codes.Length && states == ClickState.Down)
                    pressed.AddRange(codes.Where(c => !pressed.Contains(c) && KeyboardDownKeys.Contains(c)));
                if (pressed.Count != codes.Length && states == ClickState.Up)
                    pressed.AddRange(codes.Where(c => !pressed.Contains(c) && KeyboardUpKeys.Contains(c)));
                if (pressed.Count != codes.Length && states == ClickState.All)
                    pressed.AddRange(codes.Where(c => !pressed.Contains(c) && KeyboardAllPressedKeys.Contains(c)));
                return pressed.Count == codes.Length;
            }
        }

        public static bool IsPressedAny(ClickState states, KeyboardKey[] codes)
        {
            lock (KeyLock)
            {
                var press = false;
                if (states == ClickState.Pressed) press = codes.Any(c => KeyboardPressedKeys.Contains(c));
                if (!press && states == ClickState.Down) press = codes.Any(c => KeyboardDownKeys.Contains(c));
                if (!press && states == ClickState.Up) press = codes.Any(c => KeyboardUpKeys.Contains(c));
                if (!press && states == ClickState.All) press = codes.Any(c => KeyboardAllPressedKeys.Contains(c));
                return press;
            }
        }

        public static bool IsPressedAll(ClickState states, MouseKey keys)
        {
            lock (KeyLock)
            {
                var pressed = new List<MouseKey>();
                var all = keys.GetUniqueFlags().ToArray();
                if (states.HasFlag(ClickState.Pressed)) pressed.AddRange(all.Where(m => MousePressedKeys.Contains(m)));
                if (pressed.Count != all.Length && states.HasFlag(ClickState.Down))
                    pressed.AddRange(all.Where(c => !pressed.Contains(c) && MouseDownKeys.Contains(c)));
                if (pressed.Count != all.Length && states.HasFlag(ClickState.Up))
                    pressed.AddRange(all.Where(c => !pressed.Contains(c) && MouseUpKeys.Contains(c)));
                if (pressed.Count != all.Length && states.HasFlag(ClickState.All))
                    pressed.AddRange(all.Where(c => !pressed.Contains(c) && MouseAllPressedKeys.Contains(c)));
                return pressed.Count == all.Length;
            }
        }

        public static bool IsPressedAny(ClickState states, MouseKey keys)
        {
            lock (KeyLock)
            {
                var all = keys.GetUniqueFlags().ToArray();
                var pressed = false;
                if (states == ClickState.Pressed) pressed = all.Any(k => MousePressedKeys.Contains(k));
                if (!pressed && states == ClickState.Down) pressed = all.Any(k => MouseDownKeys.Contains(k));
                if (!pressed && states == ClickState.Up) pressed = all.Any(k => MouseUpKeys.Contains(k));
                if (!pressed && states == ClickState.All) pressed = all.Any(k => MouseAllPressedKeys.Contains(k));
                return pressed;
            }
        }

        public static void CallSync(ActionCallback action)
        {
            lock (SyncLock)
            {
                SyncActions.Enqueue(action);
            }
        }


        private static void AsyncFixedTick()
        {
            const int delay = 1000 / Undefined.FixedTicksPerSecond;
            var remainMs = 0;
            while (IsAsyncFixedTickWorking)
            {
                var delta = delay - remainMs < 1 ? 1 : delay - remainMs;
                Thread.Sleep(delta);
                var now = DateTime.Now;
                EventManager.CallEvent(new AsyncFixedTickEvent(1f / Undefined.FixedTicksPerSecond));
                remainMs = (int)(DateTime.Now - now).TotalMilliseconds;
            }
        }

        private static void AsyncOneSecondTick()
        {
            const int delay = 1000;
            var remainMs = 0;
            while (IsOneSecondTickWorking)
            {
                var delta = delay - remainMs < 1 ? 1 : delay - remainMs;
                Thread.Sleep(delta);
                var now = DateTime.Now;
                EventManager.CallEvent(new OneSecondTickEvent());
                remainMs = (int)(DateTime.Now - now).TotalMilliseconds;
            }
        }

        private static void AsyncTick()
        {
            while (IsAsyncTickWorking)
            {
                Thread.Sleep(1);
                if (_event is null) continue;
                KeyboardKey[] pressed;
                KeyboardKey[] down;
                KeyboardKey[] up;
                lock (KeyLock)
                {
                    pressed = new KeyboardKey[KeyboardPressedKeys.Count];
                    down = new KeyboardKey[KeyboardDownKeys.Count];
                    up = new KeyboardKey[KeyboardUpKeys.Count];
                    KeyboardPressedKeys.CopyTo(pressed);
                    KeyboardDownKeys.CopyTo(down);
                    KeyboardUpKeys.CopyTo(up);
                }

                foreach (var key in pressed) EventManager.CallEvent(new KeyboardKeyPressEvent(key));
                foreach (var key in down) EventManager.CallEvent(new KeyboardKeyDownEvent(key));
                foreach (var key in up) EventManager.CallEvent(new KeyboardKeyUpEvent(key));
                EventManager.CallEvent(new TickEvent(_event.DeltaTime));
                _event = null;
            }
        }
    }
}