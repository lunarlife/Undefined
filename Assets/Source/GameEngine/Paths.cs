using System;
using System.IO;
using UnityEngine;

namespace GameEngine
{
    public static class Paths
    {
        static Paths()
        {
            DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Undefined");
            SettingsFile = Path.Combine(DataPath, "settings.json");
            LogsPath = Path.Combine(Environment.CurrentDirectory, "Logs");
            ExternalResources = Path.Combine(DataPath, "ExternalResources");
            InternalResources = Path.Combine(Application.dataPath, "InternalResources");
            CheckPaths();
        }

        public static string SettingsFile { get; }
        public static string DataPath { get; }
        public static string LogsPath { get; }
        public static string ExternalResources { get; }
        public static string InternalResources { get; }

        public static void CheckPaths()
        {
            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
            if (!Directory.Exists(ExternalResources)) Directory.CreateDirectory(ExternalResources);
        }
    }
}