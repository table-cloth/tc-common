using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton manager sample.
/// </summary>
public class SampleSingletonManager : MonoBehaviour
{
    [SerializeField]
    private SampleSingleton singleton;

    [SerializeField]
    private SampleMonoSingleton monoSingleton;

    [SerializeField]
    private SamplePersistentMonoSingleton persistentMonoSingleton;

    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        // Get instance of singleton every frame to check.
        singleton = SampleSingleton.Instance;
        monoSingleton = SampleMonoSingleton.Instance;
        persistentMonoSingleton = SamplePersistentMonoSingleton.Instance;

        if(singleton == null)
        {
            Debug.LogError("SampleSingleton is null.");
        }
        if(monoSingleton == null)
        {
            Debug.LogError("SampleMonoSingleton is null.");
        }
        if(persistentMonoSingleton == null)
        {
            Debug.LogError("SamplePersistentMonoSingleton is null.");
        }
    }
}
