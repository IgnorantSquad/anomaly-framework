using UnityEngine;

namespace Anomaly
{
    public abstract class BaseEvent
    {
        public CustomBehaviour sender;
        public CustomBehaviour receiver;

        public abstract void Invoke();
    }
}