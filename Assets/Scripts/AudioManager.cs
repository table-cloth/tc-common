using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

public class AudioManager : MonoSingleton<AudioManager>
{
    // Pool
    private SingletonPool sePool = null;
    private SingletonPool bgmPool = null;
    private PoolableObject audioPoolableObject = null;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        GameObject gameObject = new GameObject();
        audioPoolableObject = gameObject.AddComponent<PoolableObject>();
        Destroy(gameObject);

        sePool = new Pool(sePool);
        bgmPool = new Pool(bgmPool);
    }
}
