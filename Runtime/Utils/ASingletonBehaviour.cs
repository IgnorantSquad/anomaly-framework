using UnityEngine;

namespace Anomaly.Utils
{
    public class ASingletonBehaviour<T> : ABehaviour where T : ABehaviour
    {
        static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance != null) return instance;

                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    instance = new GameObject($"__TEMP_SINGLETON_{typeof(T).ToString()}").AddComponent<T>();
                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }
    }
}