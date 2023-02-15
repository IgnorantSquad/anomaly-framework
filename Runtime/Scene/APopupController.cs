namespace Anomaly
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Anomaly.Utils;

    public class APopupController : AUIController<APopup>
    {
        private static Stack<APopup> popupStack = new Stack<APopup>();

        private static bool isAnimating = false;

        public static void Show(string popupName, AUIEventParam param = null)
        {
            Debug.Assert(layoutDictionary.ContainsKey(popupName));

            ASmartCoroutine.Create(CoExecute());

            IEnumerator CoExecute()
            {
                while (isAnimating) yield return null;

                popupStack.Push(layoutDictionary[popupName]);

                Current = popupStack.Peek();
                Current.gameObject.SetActive(true);

                isAnimating = true;
                yield return Current.OnEnter(param);
                isAnimating = false;
            }
        }

        public static void Hide()
        {
            if (popupStack.Count == 0) return;

            ASmartCoroutine.Create(CoExecute());

            IEnumerator CoExecute()
            {
                while (isAnimating) yield return null;

                isAnimating = true;
                yield return popupStack.Peek().OnExit();
                isAnimating = false;

                popupStack.Peek().gameObject.SetActive(false);
                popupStack.Pop();

                Current = popupStack.Count == 0 ? null : popupStack.Peek();
            }
        }


        public static IEnumerator Release()
        {
            while (popupStack.Count > 0)
            {
                yield return popupStack.Pop().OnExit();
            }
        }


        public static T GetPopupObject<T>(string name) where T : APopup
        {
            return layoutDictionary[name] as T;
        }
    }
}