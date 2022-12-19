using System;
using System.IO;

namespace GameEngine
{

    public static class Paths
    {
        static Paths()
        {
            DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Undefined");
            SettingsFile = Path.Combine(DataPath, "settings.json");
            LogsPath = Path.Combine(Environment.CurrentDirectory, "Logs");
        }

        public static string SettingsFile { get; }
        public static string DataPath { get; }
        public static string LogsPath { get; }

        public static void CheckPaths()
        {
            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
        }
    }
}