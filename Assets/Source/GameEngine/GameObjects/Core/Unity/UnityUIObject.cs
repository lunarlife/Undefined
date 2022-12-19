namespace GameEngine.GameObjects.Core.Unity
{
    public class UnityUIObject : UnityObject
    {
        public UnityEngine.RectTransform Transform { get; private set; }

        protected override void Awake()
        {
            Transform = gameObject.AddComponent<UnityEngine.RectTransform>();
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