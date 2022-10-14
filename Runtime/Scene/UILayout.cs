namespace Anomaly
{
    using System.Collections;
    using UnityEngine;

    public class UIEventParam
    {

    }

    public abstract class UILayout : MonoBehaviour
    {
        public abstract IEnumerator OnEnter(UIEventParam param);
        public abstract IEnumerator OnExit();
    }
}