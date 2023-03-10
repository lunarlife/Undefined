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
        private static Resolution _resolutionUnscaled = Resolution;
        public static Resolution MinResolution { get; } = new() { Width = 800, Height = 600 };
        public static Resolution Resolution { get; } = new() { Width = 1920, Height = 1080 };
        public static Resolution MaxResolution { get; } = new() { Width = 4096, Height = 3072 };
        public static Event<ResolutionChangedEvent> OnResolutionChanged { get; } = new();
        public static Resolution ResolutionUnscaled
        {
            get => _resolutionUnscaled;
            set
            {
                if (value.Width < MinResolution.Width || value.Height < MinResolution.Height)
                    throw new SettingsException(
                        $"{nameof(GameSettings.Resolution)} cant be lower then {MinResolution.Width}x{MinResolution.Height}");
                _resolutionUnscaled = value;
                Undefined.CallSynchronously(() => Screen.SetResolution(value.Width, value.Height, value.IsFullScreen));
                Save();
                OnResolutionChanged.Invoke(new ResolutionChangedEvent());
            }
        }

        public static Volume Volume { get; }

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
            }));
        }

        private struct SettingsSave
        {
            public Resolution Resolution;
            public Volume Volume;
            public BindsSettings Binds;
        }
    }

    public class SettingsException : Exception
    {
        public SettingsException(string msg) : base(msg)
        {
        }
    }
}