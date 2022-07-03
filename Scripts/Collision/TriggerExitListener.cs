using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class TriggerExitListener : CustomBehaviour
    {
        public UnityEvent<Collider> onExit;

        private void OnTriggerExit(Collider other)
        {
            onExit?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}