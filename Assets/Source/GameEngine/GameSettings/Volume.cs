using Events.SettingsEvents;
using Utils;
using Utils.Events;

namespace GameEngine.GameSettings
{
    public struct Volume
    {
        private float _music;
        private float _environment;
        private float _interface;
        public Event<VolumeChangeEvent> OnChanged { get; }
        public float Music
        {
            get => _music;
            set
            {
                if (!MathUtils.Clamp(value, 0, 100)) throw new SettingsException("music out of range");
                _music = value;
                Settings.Save();
                OnChanged.Invoke(new VolumeChangeEvent());
            }
        }

        public float Environment
        {
            get => _environment;
            set
            {
                if (!MathUtils.Clamp(value, 0, 100)) throw new SettingsException("environment out of range");
                _environment = value;
                Settings.Save();
                OnChanged.Invoke(new VolumeChangeEvent());
            }
        }

        public float Interface
        {
            get => _interface;
            set
            {
                if (!MathUtils.Clamp(value, 0, 100)) throw new SettingsException("interface out of range");
                _interface = value;
                Settings.Save();
                OnChanged.Invoke(new VolumeChangeEvent());
            }
        }

        public Volume(float music, float environment, float @interface)
        {
            if (!MathUtils.Clamp(music, 0, 100)) throw new SettingsException("music out of range");
            if (!MathUtils.Clamp(environment, 0, 100)) throw new SettingsException("environment out of range");
            if (!MathUtils.Clamp(@interface, 0, 100)) throw new SettingsException("interface out of range");
            _music = music;
            _environment = environment;
            _interface = @interface;
            OnChanged = new Event<VolumeChangeEvent>();
        }
    }
}