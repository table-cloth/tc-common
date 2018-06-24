using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton manager sample.
/// </summary>
public class SingletonManager : MonoBehaviour
{
    /// <summary>
    /// Update this instance.
    /// </summary>
    private void Update()
    {
        // Get instance of singleton every frame to check.
        SampleSingleton singleton = SampleSingleton.Instance;
        SampleMonoSingleton monoSingleton = SampleMonoSingleton.Instance;
        SamplePersistentMonoSingleton persistentMonoSingleton = SamplePersistentMonoSingleton.Instance;

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
