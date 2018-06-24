using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

public class SampleSingleton : Singleton<SampleSingleton>
{
    public SampleSingleton()
    {
        Debug.Log("Constructor called for " + GetType().FullName);
    }
}
