using System.Collections;
using UnityEngine;

namespace Anomaly.Utils
{
    public partial class ASmartCoroutine
    {
        public static ASmartCoroutine AfterDelay(float delay, System.Action body)
        {
            return Create(CoInner).Start();

            IEnumerator CoInner()
            {
                yield return new WaitForSeconds(delay);
                body?.Invoke();
            }
        }

        public static ASmartCoroutine AfterOneFrame(System.Action body)
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