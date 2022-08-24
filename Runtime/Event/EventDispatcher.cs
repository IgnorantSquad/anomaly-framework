using System.Collections.Generic;

namespace Anomaly
{
    public class EventDispatcher
    {
        private EventDispatcher() { }

        private static EventDispatcher instance = null;
        public static EventDispatcher Instance => instance ?? (instance = new EventDispatcher());

        private List<BaseEvent> eventList = new List<BaseEvent>();
        public object eventListLock = new object();

        public BaseEvent Get(int index)
        {
            if (index < 0 || index >= eventList.Count)
            {
                throw new System.IndexOutOfRangeException();
            }

            return eventList[index];
        }
        public int Count => eventList.Count;
        public bool IsEmpty => eventList.Count == 0;


        public void Send(BaseEvent param = null)
        {
            eventList.Add(param);
        }


        public void Remove(int index)
        {
            lock (eventListLock)
            {
                eventList[index] = eventList[eventList.Count - 1];
                eventList.RemoveAt(eventList.Count - 1);
            }
        }
    }
}