namespace Anomaly
{
    using System.Collections;
    using UnityEngine;

    public class AUIEventParam
    {

    }

    public abstract class AUILayout : MonoBehaviour
    {
        public abstract IEnumerator OnEnter(AUIEventParam param);
        public abstract IEnumerator OnExit();
    }
}