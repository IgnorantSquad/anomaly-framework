namespace Anomaly
{
    using System.Collections;
    using UnityEngine;
    using Anomaly.Utils;

    public class OverlayController : UIController<Overlay>
    {
        public static void Change(string name, UIEventParam param = null)
        {
            Debug.Assert(layoutDictionary.ContainsKey(name));

            SmartCoroutine.Create(CoExecute());

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