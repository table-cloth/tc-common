using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC
{
    /// <summary>
    /// Class for pooling objects.
    /// </summary>
    public class PoolableObject : MonoBehaviour
    {

        // Pool where this object is spawned from.
        private Pool pool;

        /// <summary>
        /// Instantiate the specified _poolableObject in _pool.
        /// </summary>
        /// <param name="_pool">Pool.</param>
        /// <param name="_poolableObject">Poolable object.</param>
        public static PoolableObject Instantiate(Pool _pool, PoolableObject _poolableObject)
        {
            PoolableObject poolableObject = Instantiate(_poolableObject);
            poolableObject.pool = _pool;
            return poolableObject;
        }

        /// <summary>
        /// Returns this poolable object to pool.
        /// </summary>
        public void Return2Pool() {
            pool.Return2Pool(this);
        }

        /// <summary>
        /// Returns this poolable object to pool after delay sec.
        /// </summary>
        public void Return2Pool(float _delay)
        {
            StartCoroutine(CoReturn2Pool(_delay));
        }

        /// <summary>
        /// Returns this poolable object to pool after delay sec.
        /// </summary>
        private IEnumerator CoReturn2Pool(float _delay)
        {
            yield return new WaitForSeconds(_delay);
            Return2Pool();
        }

    }
}
