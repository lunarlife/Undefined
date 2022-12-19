namespace GameEngine.GameObjects.Core.Unity
{
    public class UnityGameObject : UnityObject
    {
        public UnityEngine.Transform Transform { get; private set; }

        protected override void Awake()
        {
            Transform = transform;
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