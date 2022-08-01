using System.Collections;
using UnityEngine;

namespace Anomaly.Utils
{
    public partial class SmartCoroutine
    {
        public static SmartCoroutine AfterDelay(float delay, System.Action body)
        {
            return Create(CoInner).Start();

            IEnumerator CoInner()
            {
                yield return new WaitForSeconds(delay);
                body?.Invoke();
            }
        }

        public static SmartCoroutine AfterOneFrame(System.Action body)
        {
            return Create(CoInner).Start();

            IEnumerator CoInner()
            {
                yield return null;
                body?.Invoke();
            }
        }
    }
}