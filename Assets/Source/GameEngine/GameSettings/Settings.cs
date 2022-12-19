using System;
using System.IO;
using Events.SettingsEvents;
using Newtonsoft.Json;
using UnityEngine;
using Utils;
using Utils.Events;

namespace GameEngine.GameSettings
{

    public static class Settings
    {
        private static int _chatMaxMessageCount = 50;
        private static float _scrollUiSpeed = 40;
        public static Resolution MinResolution { get; } = new() { Width = 800, Height = 600 };
        public static Resolution Resolution { get; } = new() { Width = 1920, Height = 1080 };
        private static Resolution _resolutionUnscaled = Resolution;
        public static Resolution MaxResolution { get; } = new() { Width = 4096, Height = 3072 };
        public static Resolution ResolutionUnscaled
        {
            get => _resolutionUnscaled;
            set
            {
                if (value.Width < MinResolution.Width || value.Height < MinResolution.Height)
                    throw new SettingsException(
                        $"{nameof(GameEngine.GameSettings.Resolution)} cant be lower then {MinResolution.Width}x{MinResolution.Height}");
                _resolutionUnscaled = value;
                Undefined.CallSynchronously(() => Screen.SetResolution(value.Width, value.Height, value.IsFullScreen));
                Save();
                EventManager.CallEvent(new ResolutionChangedEvent());
            }
        }

        public static Volume Volume { get; }

        public static int ChatMaxMessageCount
        {
            get => _chatMaxMessageCount;
            set
            {
                _chatMaxMessageCount = MathUtils.Clamp(value, 10, 200)
                    ? value
                    : throw new SettingsException("Message count out of range");
                Save();
                EventManager.CallEvent(new ChatMessageCountChangeEvent());
            }
        }

        public static BindsSettings Binds { get; } = new();
        

        public static float ScrollUiSpeed
        {
            get => _scrollUiSpeed;
            set
            {
                _scrollUiSpeed = value;
                Save();
            }
        }

        public static async void Save()
        {
            if (!Directory.Exists(Path.GetDirectoryName(Paths.SettingsFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(Paths.SettingsFile)!);
            await File.WriteAllTextAsync(Paths.SettingsFile, JsonConvert.SerializeObject(new SettingsSave
            {
                Resolution = _resolutionUnscaled,
                Volume = Volume,
                Binds = Binds,
                ChatMaxMessageCount = _chatMaxMessageCount,
            }));
        }

        private struct SettingsSave
        {
            public Resolution Resolution;
            public Volume Volume;
            public BindsSettings Binds;
            public int ChatMaxMessageCount;
        }
    }

    public class SettingsException : Exception
    {
        public SettingsException(string msg) : base(msg)
        {
        }
    }
}