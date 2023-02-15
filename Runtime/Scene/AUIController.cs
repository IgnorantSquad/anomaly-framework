namespace Anomaly
{
    using System.Collections.Generic;
    using UnityEngine;
    using Anomaly.Utils;

    public class AUIController<T> : MonoBehaviour where T : AUILayout
    {
        protected static Dictionary<string, T> layoutDictionary = new Dictionary<string, T>();
        public static T Current { get; protected set; }

        [SerializeField] private T firstLayout;

        protected virtual void Awake()
        {
            layoutDictionary.Clear();
            Current = null;

            var layouts = GetComponentsInChildren<T>(true);
            for (int i = 0; i < layouts.Length; ++i)
            {
                layoutDictionary.Add(layouts[i].name, layouts[i]);
                if (!layouts[i].gameObject.activeSelf) continue;
                Current = layouts[i];
            }

            if (firstLayout != null) Current = firstLayout;

            if (Current == null) return;

            ASmartCoroutine.Create(Current.OnEnter(null));
        }
    }
}