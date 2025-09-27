using UnityEngine;
using UnityEngine.Pool;

namespace Spawners
{
    public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] protected T Prefab;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private int _poolMaxSize;

        private ObjectPool<T> _objectPool;

        private void Awake()
        {
            _objectPool = new ObjectPool<T>
                (
                createFunc: () => CreateFunc(),
                actionOnGet: (@object) => ChangeParameters(@object),
                actionOnRelease: (@object) => @object.gameObject.SetActive(false),
                actionOnDestroy: (@object) => DestroyObject(@object),
                collectionCheck: true,
                defaultCapacity: _poolCapacity,
                maxSize: _poolMaxSize
                );
        }

        public abstract T CreateFunc();

        public virtual void DestroyObject(T @object)
        {
            Destroy(@object.gameObject);
        }

        public virtual void ReleaseObjectToPool(T @object)
        {
            _objectPool.Release(@object);
        }

        public virtual void ChangeParameters(T @object)
        {
            @object.gameObject.SetActive(true);
        }

        public void SpawnObject()
        {
            _objectPool.Get();
        }
    }
}
