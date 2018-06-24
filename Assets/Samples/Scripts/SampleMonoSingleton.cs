using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

public class SampleMonoSingleton : MonoSingleton<SampleMonoSingleton>
{
    public SampleMonoSingleton()
    {
        Debug.Log("Constructor called for " + GetType().FullName);
    }
}
