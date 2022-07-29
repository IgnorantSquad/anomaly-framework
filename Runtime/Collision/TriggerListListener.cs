using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public class TriggerListListener : CustomBehaviour
    {
        private List<Collider> triggerList = new List<Collider>();
        public List<Collider> List => triggerList;

        private void OnTriggerEnter(Collider other)
        {
            triggerList.Add(other);
        }

        private void OnTriggerExit(Collider other)
        {
            triggerList.Remove(other);
        }


        protected override void Initialize() { }
    }
}