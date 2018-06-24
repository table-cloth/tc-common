using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

public class SamplePersistentMonoSingleton : PersistentMonoSingleton<SamplePersistentMonoSingleton>
{
    public SamplePersistentMonoSingleton()
    {
        Debug.Log("Constructor called for " + GetType().FullName);
    }
}
