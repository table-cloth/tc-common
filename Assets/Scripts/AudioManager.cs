using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TC;

public class AudioManager : MonoSingleton<AudioManager>
{
//    // Pool
//    private Pool<AudioSourcePoolableObject> sePool = null;
//    private Pool<AudioSourcePoolableObject> bgmPool = null;
//    private AudioSourcePoolableObject audioPoolableObject = null;
//
//    // Audio clips
//    private Dictionary<string, AudioClip> audioClipsCache = new Dictionary<string, AudioClip>();
//
//    // Audio type
//    private enum AudioType
//    {
//        BGM,
//        SE,
//    }
//
//    /// <summary>
//    /// Awake this instance.
//    /// </summary>
//    protected override void Awake()
//    {
//        base.Awake();
//        InitializePool();
//
//        for(int i = 0; i < SampleAudioData.BGMResourceFilePaths.Length; i++)
//        {
//            string audioFilePath = SampleAudioData.BGMResourceFilePaths[i];
//            GetOrCreate(AudioType.BGM, audioFilePath);
//        }
//        for(int i = 0; i < SampleAudioData.SEResourceFilePaths.Length; i++)
//        {
//            string audioFilePath = SampleAudioData.SEResourceFilePaths[i];
//            GetOrCreate(AudioType.SE, audioFilePath);
//        }
//    }
//
//    /// <summary>
//    /// Initializes the pool.
//    /// </summary>
//    private void InitializePool()
//    {
//        GameObject gameObject = new GameObject();
//        audioPoolableObject = gameObject.AddComponent<AudioSourcePoolableObject>();
//        Destroy(gameObject);
//
//        sePool = new Pool<AudioSourcePoolableObject>();
//        bgmPool = new Pool<AudioSourcePoolableObject>();
//    }
//
//    private AudioSourcePoolableObject GetOrCreate(AudioType _audioType, string _audioResourceFilePath)
//    {
////        List<AudioSourcePoolableObject> audioSourceList =
////            _audioType == AudioType.BGM
////            ? bgmPool.GetAllObjectsInPool() as List<AudioSourcePoolableObject>
////            : sePool.GetAllObjectsInPool() as List<AudioSourcePoolableObject>;
//
////        AudioSourcePoolableObject poolObject =
////            _audioType == AudioType.BGM
////            ? bgmPool.GetOrCreate(transform) as AudioSourcePoolableObject
////            : sePool.GetOrCreate(transform) as AudioSourcePoolableObject;
////        poolObject.SetAudioClip(LoadAudioClip(_audioResourceFilePath));
////
////        return poolObject;
//        return null;
//    }
//
//    private AudioClip LoadAudioClip(string _audioResourceFilePath)
//    {
//        AudioClip clip = null;
//        // Use cache if exists.
//        if(audioClipsCache.ContainsKey(_audioResourceFilePath))
//        {
//            clip = audioClipsCache[_audioResourceFilePath];
//            if(clip != null)
//            {
//                return clip;
//            }
//        }
//
//        // Load from resources.
//        clip = Resources.Load<AudioClip>(_audioResourceFilePath);
//        if(clip == null)
//        {
//            Debug.LogError("Audio clip not found with resource file path : " + _audioResourceFilePath);
//        }
//
//        return clip;
//
//    }
//
//    /// <summary>
//    /// Audio source poolable object.
//    /// Will need separate from simple PoolableObject class to add RequireComponent.
//    /// </summary>
//    [RequireComponent(typeof(AudioSource))]
//    private class AudioSourcePoolableObject : PoolableObject
//    {
//        private AudioSource audioSource;
//
//        private void Awake()
//        {
//            audioSource = GetComponent<AudioSource>();
//        }
//
//        public void test()
//        {
//            Debug.Log("Name = " + audioSource.clip.name);
////            audioSource.cl
//        }
//
//        public void SetAudioClip(AudioClip _clip)
//        {
//            audioSource.clip = _clip;
//            test();
//        }
//    }
}
