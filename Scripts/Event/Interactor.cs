using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public class Interactor : MonoBehaviour
    {
        private static Interactor instance = null;
        
        private Queue<(BaseEvent targetEvent, EventParam param)> eventQueue = new Queue<(BaseEvent, EventParam)>();

        public static void AddEvent(BaseEvent e, EventParam p)
        {
            instance.eventQueue.Enqueue((e, p));
        }


        private void Awake() 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            while (eventQueue.Count > 0)
            {
                var current = eventQueue.Dequeue();
                current.targetEvent.Invoke(current.param);
            }
        }
    }
}