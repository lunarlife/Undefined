using UndefinedNetworking.GameEngine.Input;

namespace GameEngine.GameSettings
{
    public class BindsSettings
    {
        private KeyBind _arrowDown = KeyboardKey.DownArrow;
        private KeyBind _arrowUp = KeyboardKey.UpArrow;

        private KeyBind _cameraBoost = new()
        {
            Keys = new[] { KeyboardKey.LeftShift, KeyboardKey.RightShift }
        };

        private KeyBind _cameraDown = KeyboardKey.S;
        private KeyBind _cameraLeft = KeyboardKey.A;
        private KeyBind _cameraRight = KeyboardKey.D;
        private KeyBind _cameraUp = KeyboardKey.W;
        private KeyBind _debug = KeyboardKey.F1;
        private KeyBind _enter = KeyboardKey.Enter;
        private KeyBind _esc = KeyboardKey.Escape;
        private KeyBind _fullScreen = KeyboardKey.F11;
        private KeyBind _inputFieldMultiSelection = KeyboardKey.LeftControl;

        private KeyBind _scrollHorizontal = new()
        {
            Keys = new[] { KeyboardKey.LeftShift, KeyboardKey.RightShift }
        };

        private KeyBind _tab = KeyboardKey.Tab;

        private KeyBind _windowInsertRectChange = new()
        {
            Keys = new[] { KeyboardKey.LeftShift, KeyboardKey.RightShift }
        };

        public KeyBind CameraUp
        {
            get => _cameraUp;
            set
            {
                _cameraUp = value;
                Settings.Save();
            }
        }

        public KeyBind CameraRight
        {
            get => _cameraRight;
            set
            {
                _cameraRight = value;
                Settings.Save();
            }
        }

        public KeyBind CameraDown
        {
            get => _cameraDown;
            set
            {
                _cameraDown = value;
                Settings.Save();
            }
        }

        public KeyBind CameraLeft
        {
            get => _cameraLeft;
            set
            {
                _cameraLeft = value;
                Settings.Save();
            }
        }

        public KeyBind CameraBoost
        {
            get => _cameraBoost;
            set
            {
                _cameraBoost = value;
                Settings.Save();
            }
        }

        public KeyBind Esc
        {
            get => _esc;
            set
            {
                _esc = value;
                Settings.Save();
            }
        }

        public KeyBind Enter
        {
            get => _enter;
            set
            {
                _enter = value;
                Settings.Save();
            }
        }

        public KeyBind Tab
        {
            get => _tab;
            set
            {
                _tab = value;
                Settings.Save();
            }
        }

        public KeyBind ArrowUp
        {
            get => _arrowUp;
            set
            {
                _arrowUp = value;
                Settings.Save();
            }
        }

        public KeyBind ArrowDown
        {
            get => _arrowDown;
            set
            {
                _arrowDown = value;
                Settings.Save();
            }
        }

        public KeyBind FullScreen
        {
            get => _fullScreen;
            set
            {
                _fullScreen = value;
                Settings.Save();
            }
        }

        public KeyBind Debug
        {
            get => _debug;
            set
            {
                _debug = value;
                Settings.Save();
            }
        }

        public KeyBind ScrollHorizontal
        {
            get => _scrollHorizontal;
            set
            {
                _scrollHorizontal = value;
                Settings.Save();
            }
        }

        public KeyBind WindowInsertRectChange
        {
            get => _windowInsertRectChange;
            set
            {
                _windowInsertRectChange = value;
                Settings.Save();
            }
        }

        public KeyBind InputFieldMultiSelection
        {
            get => _inputFieldMultiSelection;
            set
            {
                _inputFieldMultiSelection = value;
                Settings.Save();
            }
        }
    }
}