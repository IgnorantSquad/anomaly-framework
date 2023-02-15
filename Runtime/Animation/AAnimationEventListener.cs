using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anomaly.Utils;

namespace Anomaly
{
    [System.Serializable]
    public class AAnimationEventCallback : UnityEngine.Events.UnityEvent<AnimationEvent> { }

    public class AAnimationEventListener : ABehaviour
    {
        public ASerializableDictionary<AAnimationEventCallback> animationEvents = new ASerializableDictionary<AAnimationEventCallback>();

        public void OnEvent(AnimationEvent param)
        {
            string function = param.stringParameter.Contains("|") ? param.stringParameter.Split('|')[0] : param.stringParameter;
            if (!animationEvents.Container.ContainsKey(function)) return;

            animationEvents.Container[function]?.Invoke(param);
        }


        // Remove useless initialization
        protected override void Initialize()
        {
        }
    }
}

