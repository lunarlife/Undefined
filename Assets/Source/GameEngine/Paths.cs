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
            ExternalResources = Path.Combine(Application.dataPath, "ExternalResources");
            ExternalSprites = Path.Combine(ExternalResources, "Sprites");
            ExternalShaders = Path.Combine(ExternalResources, "Shaders");
            CheckPaths();
        }

        public static string SettingsFile { get; }
        public static string DataPath { get; }
        public static string LogsPath { get; }
        public static string ExternalResources { get; }
        public static string ExternalSprites { get; }
        public static string ExternalShaders { get; }
        public static void CheckPaths()
        {
            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
            if (!Directory.Exists(ExternalResources)) Directory.CreateDirectory(ExternalResources);
            if (!Directory.Exists(ExternalSprites)) Directory.CreateDirectory(ExternalSprites);
            if (!Directory.Exists(ExternalShaders)) Directory.CreateDirectory(DataPath);
        }
    }
}