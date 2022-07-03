using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class CollisionStayListener : CustomBehaviour
    {
        public UnityEvent<Collision> onStay;

        private void OnCollisionStay(Collision other)
        {
            onStay?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}