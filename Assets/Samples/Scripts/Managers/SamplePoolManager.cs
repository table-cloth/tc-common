using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

/// <summary>
/// Sample pool manager.
/// </summary>
public class SamplePoolManager : MonoBehaviour
{
    private const float SpawnDelay = 0.1f;
    private const float DestroyDelay = 1.0f;

    [SerializeField]
    private PoolableObject poolableObject;
    private Pool pool;

    /// <summary>
    /// Start this instance.
    /// </summary>
    public void Start()    
    {
        pool = new Pool(poolableObject);
        StartCoroutine(RepeatSpawnPoolableObject(SpawnDelay));
    }

    /// <summary>
    /// Repeats the spawn poolable object.
    /// </summary>
    /// <returns>The spawn poolable object.</returns>
    /// <param name="_delay">Delay.</param>
    private IEnumerator RepeatSpawnPoolableObject(float _delay)
    {
        yield return new WaitForSeconds(_delay);

        PoolableObject obj = pool.GetOrCreate(this.transform);

        float randX = Random.Range(0.0f, 1.0f);
        float randY = Random.Range(0.0f, 1.0f);
        float randZ = Random.Range(0.0f, 1.0f);
        obj.GetComponent<Rigidbody>().velocity = new Vector3(randX, randY, randZ);
        obj.transform.localPosition = Vector3.zero;

        obj.Return2Pool(DestroyDelay);

        StartCoroutine(RepeatSpawnPoolableObject(_delay));
    }
}
