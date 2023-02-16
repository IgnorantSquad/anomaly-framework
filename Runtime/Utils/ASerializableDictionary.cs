using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [System.Serializable]
    public partial class ASerializableDictionary<_Typ> : ISerializationCallbackReceiver
    {
        #region Constructor
        public ASerializableDictionary() { }
        public ASerializableDictionary(string defaultKey, _Typ defaultValue = default(_Typ))
        {
            keyList = new string[] { defaultKey };
            valueList = new _Typ[] { defaultValue };
            OnAfterDeserialize();
        }
        #endregion

        public Dictionary<string, _Typ> Container { get; set; } = new Dictionary<string, _Typ>();

        public _Typ Get(string key)
        {
            _Typ value = default(_Typ);
            Debug.Assert(Container.TryGetValue(key, out value));
            return value;
        }

        #region Serialization
        [SerializeField] private string[] keyList;
        [SerializeField] private _Typ[] valueList;

        public void OnAfterDeserialize()
        {
            Container.Clear();

            for (int i = 0; i < Mathf.Min(keyList.Length, valueList.Length); ++i)
            {
                // Avoid conflict
                if (Container.ContainsKey(keyList[i]))
                {
                    keyList[i] = System.Guid.NewGuid().ToString();
                }
                Container.Add(keyList[i], valueList[i]);
            }

            keyList = null;
            valueList = null;
        }
        public void OnBeforeSerialize()
        {
            keyList = new string[Container.Keys.Count];
            valueList = new _Typ[Container.Values.Count];

            Container.Keys.CopyTo(keyList, 0);
            Container.Values.CopyTo(valueList, 0);
        }
        #endregion
    }

    public class AnimationEventDictionary
    {

    }
}
