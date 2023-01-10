using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Logger = Networking.Loggers.Logger;

namespace GameEngine
{
    public class ULogger : Logger
    {
        private static readonly ULogger _instance;

        static ULogger()
        {
            _instance = new ULogger();
            _instance.CheckLogFile();
            File.WriteAllText(Path.Combine(Paths.LogsPath, "latest.log"), "");
        }

        public static void ShowInfo(string info)
        {
            _instance.Info(info);
        }

        public static void ShowWarning(string info)
        {
            _instance.Warning(info);
        }

        public static void ShowError(string info)
        {
            _instance.Error(info);
        }

        private void WriteToFile(string log)
        {
            CheckLogFile();
            File.AppendAllText(Path.Combine(Paths.LogsPath, "latest.log"), log + "\n");
        }

        public override void Info(string info)
        {
            WriteToFile(info);
            Debug.Log(info);
        }

        public override void Warning(string warning)
        {
            WriteToFile(warning);
            Debug.LogWarning(warning);
        }

        public override void Error(string error)
        {
            WriteToFile(error);
            Debug.LogError(error);
        }

        public override void Error(Exception e)
        {
            switch (e)
            {
                case TargetInvocationException { InnerException: { } } invocationException:
                    Undefined.Logger.Error($"{invocationException.InnerException.Message}\n{invocationException.InnerException.StackTrace}");
                    break;
                default:
                    Undefined.Logger.Error($"{e.Message}\n{e.StackTrace}");
                    break;
            }
        }

        private void CheckLogFile()
        {
            if (!Directory.Exists(Paths.LogsPath)) Directory.CreateDirectory(Paths.LogsPath);
        }
    }
}