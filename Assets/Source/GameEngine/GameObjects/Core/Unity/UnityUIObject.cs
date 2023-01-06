using UnityEngine;

namespace GameEngine.GameObjects.Core.Unity
{
    public class UnityUIObject : UnityObject
    {
        public RectTransform Transform { get; private set; }

        protected override void Awake()
        {
            Transform = gameObject.AddComponent<RectTransform>();
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }
    }
}