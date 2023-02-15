using UnityEngine;

namespace Anomaly
{
    public abstract class AEvent
    {
        public ABehaviour sender;
        public ABehaviour receiver;

        public abstract void Invoke();
    }
}