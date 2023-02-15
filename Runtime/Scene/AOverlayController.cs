namespace Anomaly
{
    using System.Collections;
    using UnityEngine;
    using Anomaly.Utils;

    public class AOverlayController : AUIController<AOverlay>
    {
        public static void Change(string name, AUIEventParam param = null)
        {
            Debug.Assert(layoutDictionary.ContainsKey(name));

            ASmartCoroutine.Create(CoExecute());

            IEnumerator CoExecute()
            {
                if (Current != null)
                {
                    yield return Current.OnExit();
                    Current.gameObject.SetActive(false);
                }

                Current = layoutDictionary[name];
                Current.gameObject.SetActive(true);
                yield return Current.OnEnter(param);
            }
        }

        public static IEnumerator Release()
        {
            if (Current == null) yield break;
            yield return Current.OnExit();
        }
    }
}