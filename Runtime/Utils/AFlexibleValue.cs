using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    [System.Serializable]
    public class AFlexibleValue
    {
        [SerializeField] private float defaultValue = default(float);

        [SerializeField] private float minValue = Mathf.NegativeInfinity, maxValue = Mathf.Infinity;
        [SerializeField] private bool hasRange = false;


        public float Addition { get; set; } = 0F;
        public float Multiplier { get; set; } = 1F;
        public float FinalAddition { get; set; } = 0F;
        public float FinalMultiplier { get; set; } = 1F;

        public float Value
        {
            get
            {
                float value = (defaultValue * Multiplier + Addition) * FinalMultiplier + FinalAddition;
                return hasRange ? Mathf.Clamp(value, minValue, maxValue) : value;
            }
        }
    }
}
