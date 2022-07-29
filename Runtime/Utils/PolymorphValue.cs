using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [System.Serializable]
    public partial class PolymorphValue<_Typ> where _Typ : struct
    {
        [SerializeField]
        private _Typ defaultValue = default(_Typ);

        [SerializeField]
        private SerializableDictionary<_Typ> otherValues = new SerializableDictionary<_Typ>();

        public _Typ Default => defaultValue;
        public _Typ Get(string key = "") => string.IsNullOrEmpty(key) ? defaultValue : otherValues.Get(key);
    }
}


