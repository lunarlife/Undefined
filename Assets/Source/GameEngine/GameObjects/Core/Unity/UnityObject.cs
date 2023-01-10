using System.Threading.Tasks;
using Events.Object;
using UnityEngine;
using Utils.Events;

namespace GameEngine.GameObjects.Core.Unity
{
    public abstract class UnityObject : MonoBehaviour
    {
        public static Transform PoolObjectsParent { get; private set; }

        public ObjectCore Object { get; set; }

        public bool IsDestroyed { get; set; }
        protected virtual void Awake()
        {
            if (PoolObjectsParent is null)
            {
                PoolObjectsParent = new GameObject("object_pool_storage").transform;
                PoolObjectsParent.gameObject.SetActive(false);
            }

            transform.SetParent(PoolObjectsParent);
        }

        protected virtual async void OnEnable()
        {
            if (!IsDestroyed || Object is null) return;
            IsDestroyed = false;
            //await Task.Run(() => EventManager.CallEvent(new ObjectInstanceEvent(Object)));
        }

        protected virtual async void OnDisable()
        {
            if (!IsDestroyed || Object is not null) return;
            IsDestroyed = true;
            transform.SetParent(PoolObjectsParent);
        }
    }
}