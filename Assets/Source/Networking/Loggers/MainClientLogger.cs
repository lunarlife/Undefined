using Debug = UnityEngine.Debug;

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
    }
}