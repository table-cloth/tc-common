using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC
{
    /// <summary>
    /// Singleton class for all non MonoBehaviour class.
    /// </summary>
    public abstract class Singleton<T> where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }

        /// <summary>
        /// Creates new instance if need.
        /// </summary>
        /// <returns><c>true</c>, if instance is successfully loaded or created, <c>false</c> otherwise.</returns>
        public static bool CreateIfNeed()
        {
            if(instance != null)
            {
                return false;
            }
            // Create instance by calling Getter.
            return Instance != null;
        }
    }

    /// <summary>
    /// Singleton class for all Monobehaviour class.
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Callback on when Instance is requested.
        protected static System.Action onRequestInstance = null;
        // Callback on when Instance is newly created.
        protected static System.Action onCreateInstance = null;
        // Callback on when Instance is found from scene and set to Instance.
        protected static System.Action onUpdateInstance = null;

        private static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    T[] validInstances = GameObject.FindObjectsOfType<T>();
                    if(validInstances == null || validInstances.Length <= 0)
                    {
                        // Create new instance.
                        GameObject gameObject = new GameObject();
                        // At this moment, instance settings will not be applied.
                        // Each settings such as name will be updated on Awake.
                        instance = gameObject.AddComponent<T>();

                        if(onCreateInstance != null)
                        {
                            onCreateInstance();
                        }
                    }
                    else
                    {
                        // Load instance from scene.
                        instance = validInstances[0];
                        if(validInstances.Length > 1)
                        {
                            Debug.Log("More than 1 instance is created. Destroying duplicate instances.");
                            for(int i = 1; i < validInstances.Length; i++)
                            {
                                Destroy(validInstances[i].gameObject);
                            }
                        }

                        if(onUpdateInstance != null)
                        {
                            onUpdateInstance();
                        }
                    }
                }

                if(onRequestInstance != null)
                {
                    onRequestInstance();
                }
                return instance;
            }
        }

        /// <summary>
        /// Creates new instance if need.
        /// </summary>
        /// <returns><c>true</c>, if instance is successfully loaded or created, <c>false</c> otherwise.</returns>
        public static bool CreateIfNeed()
        {
            if(instance != null)
            {
                return false;
            }
            // Create instance by calling Getter.
            return Instance != null;
        }


        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected virtual void Awake()
        {
            InitializeInstance();
        }

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected virtual void InitializeInstance()
        {
            gameObject.name = this.GetType().FullName;
        }
    }

    /// <summary>
    /// Persistent singleton.
    /// This class will not be destroyed on load.
    /// </summary>
    public abstract class PersistentMonoSingleton<T> : MonoSingleton<T> where T : MonoBehaviour
    {
        /// <summary>
        /// Awake this instance.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            GameObject.DontDestroyOnLoad(Instance);
        }
    }
}

