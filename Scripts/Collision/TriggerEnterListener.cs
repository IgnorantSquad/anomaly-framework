using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class TriggerEnterListener : CustomBehaviour
    {
        public UnityEvent<Collider> onEnter;

        private void OnTriggerEnter(Collider other)
        {
            onEnter?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}