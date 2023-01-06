using System;
using System.Reflection;

namespace GameEngine
{
    public class ActionCallback
    {
        private MethodInfo _callbackMethod;
        private object _callbackTarget;

        private MethodInfo _invokeMethod;
        private object _invokeTarget;

        public ActionCallback(Action action, Action callback = null)
        {
            _callbackMethod = callback?.Method;
            _callbackTarget = callback?.Target;
            _invokeMethod = action.Method;
            _invokeTarget = action.Target;
        }

        private ActionCallback()
        {
        }

        public void Invoke()
        {
            var invoke = _invokeMethod.Invoke(_invokeTarget, null);
            _callbackMethod?.Invoke(_callbackTarget, invoke is null ? null : new[] { invoke });
        }

        public static ActionCallback Of<T>(Func<T> func, Action<T> callback)
        {
            return new()
            {
                _invokeMethod = func.Method,
                _invokeTarget = func.Target,
                _callbackMethod = callback.Method,
                _callbackTarget = callback.Target
            };
        }

        public static implicit operator ActionCallback(Action action)
        {
            return new(action);
        }
    }
}