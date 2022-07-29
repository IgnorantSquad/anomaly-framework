using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class CollisionExitListener : CustomBehaviour
    {
        public UnityEvent<Collision> onExit;

        private void OnCollisionExit(Collision other)
        {
            onExit?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}