using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anomaly.Utils;

namespace Anomaly
{
    [System.Serializable]
    public class AnimationEventCallback : UnityEngine.Events.UnityEvent<AnimationEvent> { }

    public class AnimationEventListener : CustomObject
    {
        // public AnimatorComponent.Data data;

        // public SerializableDictionary<AnimationEventCallback> animationEvents = new SerializableDictionary<AnimationEventCallback>();

        // public void OnEvent(AnimationEvent param)
        // {
        //     string function = param.stringParameter.Contains("|") ? param.stringParameter.Split('|')[0] : param.stringParameter;
        //     if (!animationEvents.Container.ContainsKey(function)) return;

        //     animationEvents.Container[function]?.Invoke(param);
        // }
    }
}

