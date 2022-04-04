using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public abstract class BaseEvent
    {
        protected Actor sender, receiver;
        public BaseEvent(Actor sender, Actor receiver)
        {
            this.sender = sender;
            this.receiver = receiver;
        }

        public abstract void Invoke();
    }
}