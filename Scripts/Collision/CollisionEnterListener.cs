using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    public class CollisionEnterListener : CustomBehaviour
    {
        public UnityEvent<Collision> onEnter;

        private void OnCollisionEnter(Collision other)
        {
            onEnter?.Invoke(other);
        }


        protected override void Initialize() { }
    }
}