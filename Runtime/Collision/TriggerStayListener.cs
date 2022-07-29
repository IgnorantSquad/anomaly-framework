using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class TriggerStayListener : CustomBehaviour
    {
        public UnityEvent<Collider> onStay;

        private void OnTriggerStay(Collider other)
        {
            onStay?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}