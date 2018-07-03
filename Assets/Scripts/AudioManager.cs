using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TC;

namespace TC
{
    public class AudioManager : PersistentMonoSingleton<AudioManager>
    {
        // Pool
        private Pool sePool = null;
        private Pool bgmPool = null;
        private Transform seParent = null;
        private Transform bgmParent = null;
        private AudioSourcePoolableObject originalAudioPoolableObject = null;

        // Audio clips
        private Dictionary<string, AudioClip> cacheAudioClips = new Dictionary<string, AudioClip>();
        // Active audio sources.
        private List<AudioSourcePoolableObject> activeSECacheList = new List<AudioSourcePoolableObject>();
        private List<AudioSourcePoolableObject> activeBGMCacheList = new List<AudioSourcePoolableObject>();

        // Audio type
        private enum AudioType
        {
            BGM,
            SE,
        }

        /// <summary>
        /// Plays SE.
        /// </summary>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        public void PlaySE(string _audioResourceFilePath)
        {
            AudioSourcePoolableObject audioPoolObj = GetOrCreate(AudioType.SE, _audioResourceFilePath);
            activeSECacheList.Add(audioPoolObj);
            audioPoolObj
                .SetOnCompleteCallback((_audioPoolObject) =>
                {
                    Debug.Log("Return SE to pool");
                    _audioPoolObject.Return2Pool();
                    activeSECacheList.Remove(_audioPoolObject);
                })
                .PlayOneShot(LoadAudioClip(_audioResourceFilePath));
        }

        /// <summary>
        /// Plays BGM.
        /// </summary>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        public void PlayBGM(string _audioResourceFilePath)
        {
            StopAllBGM();
            AudioSourcePoolableObject audioPoolObj = GetOrCreate(AudioType.BGM, _audioResourceFilePath);
            activeBGMCacheList.Add(audioPoolObj);
            audioPoolObj
                .SetOnCompleteCallback((_audioPoolObject) =>
                {
                    Debug.Log("Return BGM to pool");
                    _audioPoolObject.Return2Pool();
                    activeBGMCacheList.Remove(_audioPoolObject);
                })
                .SetAudioClip(LoadAudioClip(_audioResourceFilePath))
                .Play(true);
        }

        /// <summary>
        /// Stops all SE.
        /// </summary>
        public void StopAllSE()
        {
            StopAllAudio(AudioType.SE);
        }

        /// <summary>
        /// Stops all BGM.
        /// </summary>
        public void StopAllBGM()
        {
            StopAllAudio(AudioType.BGM);
        }

        /// <summary>
        /// Loads the audio clip2 cache.
        /// This needs to be done before playing any audio files.
        /// </summary>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        public void LoadAudioClip2Cache(string _audioResourceFilePath)
        {
            // Do nothing if cache exists.
            if(cacheAudioClips.ContainsKey(_audioResourceFilePath))
            {
                return;
            }

            AudioClip clip = LoadAudioClip(_audioResourceFilePath);
            cacheAudioClips.Add(_audioResourceFilePath, clip);
        }

        /// <summary>
        /// Clears all AudioClip and Pool.
        /// Audio currently played, will not be stopped nor cleared.
        /// </summary>
        public void ClearAllData()
        {
            ClearAudioClipCache();
            ClearPool();
        }

        /// <summary>
        /// Clears the audio clip cache.
        /// </summary>
        public void ClearAudioClipCache()
        {
            cacheAudioClips.Clear();
        }

        /// <summary>
        /// Clears the pool.
        /// </summary>
        public void ClearPool()
        {
            bgmPool.Clear();
            sePool.Clear();
        }

        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            InitializePool();
        }

        /// <summary>
        /// Finds the active SE.
        /// </summary>
        /// <returns>The active S.</returns>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        private AudioSourcePoolableObject FindActiveSE(string _audioResourceFilePath)
        {
            return FindActiveAudio(AudioType.SE, _audioResourceFilePath);
        }

        /// <summary>
        /// Finds the active BGM.
        /// </summary>
        /// <returns>The active background.</returns>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        private AudioSourcePoolableObject FindActiveBGM(string _audioResourceFilePath)
        {
            return FindActiveAudio(AudioType.BGM, _audioResourceFilePath);
        }

        private List<AudioSourcePoolableObject> GetAudioCacheList(AudioType _audioType)
        {
            return _audioType == AudioType.BGM ? activeBGMCacheList : activeSECacheList;
        }

        /// <summary>
        /// Finds the active audio.
        /// </summary>
        /// <returns>The active audio.</returns>
        /// <param name="_audioType">Audio type.</param>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        private AudioSourcePoolableObject FindActiveAudio(AudioType _audioType, string _audioResourceFilePath)
        {
            // If no cache is found, will assume there is no active SE with given filePath.
            if(!cacheAudioClips.ContainsKey(_audioResourceFilePath))
            {
                return null;
            }

            string audioName = cacheAudioClips[_audioResourceFilePath].name;
            List<AudioSourcePoolableObject> activeAudioCacheList = GetAudioCacheList(_audioType);
            foreach(AudioSourcePoolableObject audio in activeAudioCacheList)
            {
                if(audio.name == audioName)
                {
                    return audio;
                }
            }

            // No audio that matches the name exists.
            return null;
        }

        /// <summary>
        /// Stops all audio.
        /// </summary>
        /// <param name="_audioPoolObjList">Audio pool object list.</param>
        private void StopAllAudio(AudioType _audioType)
        {
            List<AudioSourcePoolableObject> activeAudioCacheList = GetAudioCacheList(_audioType);
            foreach(AudioSourcePoolableObject audioPoolObj in activeAudioCacheList)
            {
                if(audioPoolObj != null)
                {
                    audioPoolObj.Stop();
                }
            }
        }

        /// <summary>
        /// Initializes the pool.
        /// </summary>
        private void InitializePool()
        {
            // Original pool object instance for cloning.
            // This object will be hidden from hierarchy, since this is only used for instantiating.
            GameObject gameObject = new GameObject();
            originalAudioPoolableObject = gameObject.AddComponent<AudioSourcePoolableObject>();
            gameObject.hideFlags = HideFlags.HideInHierarchy;

            // Parent for each pool objects.
            bgmParent = new GameObject().transform;
            bgmParent.name = "BGM";
            bgmParent.SetParent(transform);
            seParent = new GameObject().transform;
            seParent.name = "SE";
            seParent.SetParent(transform);

            // Pool instance.
            sePool = new Pool(originalAudioPoolableObject);
            bgmPool = new Pool(originalAudioPoolableObject);
        }

        /// <summary>
        /// Gets the or create AudioSourcePoolableObject.
        /// </summary>
        /// <returns>The or create.</returns>
        /// <param name="_audioType">Audio type.</param>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        private AudioSourcePoolableObject GetOrCreate(AudioType _audioType, string _audioResourceFilePath)
        {
            AudioSourcePoolableObject poolObject =
                _audioType == AudioType.BGM
                ? bgmPool.GetOrCreate(bgmParent) as AudioSourcePoolableObject
                : sePool.GetOrCreate(seParent) as AudioSourcePoolableObject;
            poolObject.SetAudioClip(LoadAudioClip(_audioResourceFilePath));

            return poolObject;
        }

        /// <summary>
        /// Loads the audio clip.
        /// If cache exists, it will return clip from cache.
        /// </summary>
        /// <returns>The audio clip.</returns>
        /// <param name="_audioResourceFilePath">Audio resource file path.</param>
        private AudioClip LoadAudioClip(string _audioResourceFilePath)
        {
            AudioClip clip = null;
            // Use cache if exists.
            if(cacheAudioClips.ContainsKey(_audioResourceFilePath))
            {
                clip = cacheAudioClips[_audioResourceFilePath];
                if(clip != null)
                {
                    return clip;
                }
            }

            // Load from resources.
            clip = Resources.Load<AudioClip>(_audioResourceFilePath);
            if(clip == null)
            {
                Debug.LogError("Audio clip not found with resource file path : " + _audioResourceFilePath);
            }

            return clip;
        }

        /// <summary>
        /// Audio source poolable object.
        /// Will need separate from simple PoolableObject class to add RequireComponent.
        /// </summary>
        [RequireComponent(typeof(AudioSource))]
        private class AudioSourcePoolableObject : PoolableObject
        {
            // AudioSource attached to this GameObject.
            private AudioSource audioSource;

            private bool isPlayTriggered = false;
            private Action<AudioSourcePoolableObject> onCompleteCallback = null;

            /// <summary>
            /// Determines whether this instance is playing.
            /// </summary>
            /// <returns><c>true</c> if this instance is playing; otherwise, <c>false</c>.</returns>
            public bool IsPlaying()
            {
                return audioSource.isPlaying;
            }

            /// <summary>
            /// Plays sound with one shot.
            /// </summary>
            /// <param name="_clip">Clip.</param>
            public void PlayOneShot(AudioClip _clip = null)
            {
                if(_clip == null)
                {
                    Debug.LogError("Given AudioClip is null.");
                    return;
                }
                Debug.Log("Play " + _clip.name);
                audioSource.PlayOneShot(_clip);
                isPlayTriggered = true;
            }

            /// <summary>
            /// Plays sound with one shot.
            /// This will play clip set to audioSource.
            /// </summary>
            public void PlayOneShot()
            {
                if(audioSource.clip == null)
                {
                    Debug.LogError("AudioClip is not set to AudioSource.");
                    return;
                }
                PlayOneShot(audioSource.clip);
                isPlayTriggered = true;
            }

            /// <summary>
            /// Play the specified isLoop.
            /// </summary>
            /// <param name="isLoop">If set to <c>true</c> is loop.</param>
            public void Play(bool _isLoop = false)
            {
                audioSource.loop = _isLoop;
                audioSource.Play();
                isPlayTriggered = true;
            }

            /// <summary>
            /// Stop this instance.
            /// </summary>
            public void Stop()
            {
                audioSource.Stop();
            }

            /// <summary>
            /// Pause this instance.
            /// </summary>
            public void Pause()
            {
                audioSource.Pause();
            }

            /// <summary>
            /// Resume this instance.
            /// </summary>
            public void Resume()
            {
                audioSource.UnPause();
            }

            /// <summary>
            /// Sets the on complete callback.
            /// </summary>
            /// <returns>This instance.</returns>
            /// <param name="_onCompleteCallback">On complete callback.</param>
            public AudioSourcePoolableObject SetOnCompleteCallback(Action<AudioSourcePoolableObject> _onCompleteCallback)
            {
                onCompleteCallback = _onCompleteCallback;
                return this;
            }

            /// <summary>
            /// Sets the audio clip.
            /// </summary>
            /// <returns>This instance.</returns>
            /// <param name="_clip">Clip.</param>
            public AudioSourcePoolableObject SetAudioClip(AudioClip _clip)
            {
                audioSource.clip = _clip;
                return this;
            }

            /// <summary>
            /// Sets the volume.
            /// Max = 1.0f.
            /// Min = 0.0f.
            /// </summary>
            /// <returns>This instance.</returns>
            /// <param name="_volume">Volume.</param>
            public AudioSourcePoolableObject SetVolume(float _volume)
            {
                audioSource.volume = _volume;
                return this;
            }

            /// <summary>
            /// Sets the pitch.
            /// </summary>
            /// <returns>This instance.</returns>
            /// <param name="_pitch">Pitch.</param>
            public AudioSourcePoolableObject SetPitch(float _pitch)
            {
                audioSource.pitch = _pitch;
                return this;
            }

            /// <summary>
            /// Awake this instance.
            /// </summary>
            private void Awake()
            {
                audioSource = GetComponent<AudioSource>();
                gameObject.name = this.GetType().Name;
            }

            /// <summary>
            /// Update this instance.
            /// </summary>
            private void Update()
            {
                // Do nothing if callback is not set.
                if(onCompleteCallback == null)
                {
                    return;
                }

                if(isPlayTriggered && !IsPlaying())
                {
                    isPlayTriggered = false;
                    onCompleteCallback(this);
                }
            }
        }
    }
}
