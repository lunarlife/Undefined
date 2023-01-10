using System;
using System.Reflection;
using GameEngine;
using Networking.Packets;
using UnityEngine;

namespace Networking.Loggers
{
    public class MainClientLogger : Logger
    {
        public override void Info(string info)
        {
            Debug.Log(info);
        }

        public override void Warning(string warning)
        {
            Debug.LogWarning(warning);
        }

        public override void Error(string error)
        {
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
    }
}