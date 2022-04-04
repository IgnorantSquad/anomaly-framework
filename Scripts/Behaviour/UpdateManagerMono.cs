using UnityEngine;

namespace Anomaly
{
    public class UpdateManagerMono : MonoBehaviour
    {
        void Awake()
        {
            if (GameObject.FindObjectsOfType<UpdateManagerMono>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        void FixedUpdate()
        {
            UpdateManager.FixedUpdate();
        }

        void Update()
        {
            UpdateManager.Update();
        }

        void LateUpdate()
        {
            UpdateManager.LateUpdate();
        }
    }
}