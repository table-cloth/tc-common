using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

namespace TC
{
    /// <summary>
    /// Class for basic pooling.
    /// Will need to be used with PoolableObject.
    /// </summary>
    public class Pool
    {

        private const int PoolSizeNoLimit = -1;

        // Object to be pooled.
        private readonly PoolableObject poolableObject;
        private readonly Queue<PoolableObject> pool;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pool"/> class.
        /// </summary>
        /// <param name="_poolableObject">Poolable object.</param>
        /// <param name="_maxPoolSize">Max pool size.</param>
        public Pool(PoolableObject _poolableObject, int _maxPoolSize = PoolSizeNoLimit)
        {
            poolableObject = _poolableObject;
            pool = _maxPoolSize == PoolSizeNoLimit
                ? new Queue<PoolableObject>()
                : new Queue<PoolableObject>(_maxPoolSize);
        }

        /// <summary>
        /// Gets the pool instance from queue or creates new pool instance.
        /// </summary>
        /// <returns>The or create.</returns>
        /// <param name="_parent">Parent.</param>
        /// <param name="_localPos">Local position.</param>
        public PoolableObject GetOrCreate(Transform _parent, Vector3 _localPos = default(Vector3))
        {
            PoolableObject poolObj = GetPoolableObject(_parent);
            if (poolObj == null) poolObj = CreatePoolableObject(_parent);

            // Initialize poolable object, no matter get or create.
            poolObj.gameObject.SetActive(true);
            poolObj.transform.localPosition = _localPos;

            return poolObj;
        }

        public List<PoolableObject> GetAllObjectsInPool()
        {
            return new List<PoolableObject>(pool);
        }

        /// <summary>
        /// Returns pool instance to the pool.
        /// </summary>
        /// <param name="_poolableObject">Poolable object.</param>
        public void Return2Pool(PoolableObject _poolableObject)
        {
            pool.Enqueue(_poolableObject);
            _poolableObject.gameObject.SetActive(false);
        }

        /// <summary>
        /// Gets the poolable object from queue.
        /// </summary>
        /// <returns>The poolable object.</returns>
        /// <param name="_parent">Parent.</param>
        private PoolableObject GetPoolableObject(Transform _parent)
        {
            if (!IsPoolObjectAvailable()) return null;

            PoolableObject poolObj = pool.Dequeue();
            poolObj.transform.SetParent(_parent);
            return poolObj;
        }

        /// <summary>
        /// Creates new poolable object.
        /// </summary>
        /// <returns>The poolable object.</returns>
        /// <param name="_parent">Parent.</param>
        private PoolableObject CreatePoolableObject(Transform _parent)
        {
            PoolableObject poolObj = PoolableObject.Instantiate(this, poolableObject);
            poolObj.transform.SetParent(_parent);
            return poolObj;
        }

        /// <summary>
        /// Determines whether reusable pool object is available in pool.
        /// </summary>
        /// <returns><c>true</c> if this instance is pool object available; otherwise, <c>false</c>.</returns>
        private bool IsPoolObjectAvailable()
        {
            return pool.Count > 0;
        }
    }

    /// <summary>
    /// Singleton pool.
    /// For using same pool from separate classes / scenes.
    /// </summary>
    public class SingletonPool : Singleton<Pool>
    {
        // No special method is needed for this class.
        // Just needs to be defined.
    }
}
