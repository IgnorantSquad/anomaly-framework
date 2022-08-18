using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Anomaly
{
    [System.Serializable]
    public class EventStream<T> where T : BaseEvent
    {
        [SerializeField]
        protected UnityEvent<T> onNotify = new UnityEvent<T>();

        protected Anomaly.Utils.Stream stream;

        public static U Create<U>(MonoBehaviour mono) where U : EventStream<T>, new()
        {
            var es = new U();
            es.Initialize(mono);
            return es;
        }

        public void Initialize(MonoBehaviour mono)
        {
            stream = Anomaly.Utils.Stream.Create(mono)
                        .Select(() => !Messenger.Instance.IsEmpty)
                        .Subscribe(d =>
                        {
                            for (int i = 0; i < Messenger.Instance.Count; ++i)
                            {
                                if (!ReferenceEquals(Messenger.Instance.Get(i).receiver, mono)) continue;

                                var cast = Messenger.Instance.Get(i) as T;
                                if (cast == null) continue;

                                Messenger.Instance.Remove(i);

                                Notify(cast);
                            }
                        });
            stream.Start();
        }


        public virtual void Notify(T e)
        {
            onNotify?.Invoke(e);
        }


        public void AddListener(UnityAction<T> listener)
        {
            onNotify.AddListener(listener);
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            onNotify.RemoveListener(listener);
        }

        public void RemoveAllListeners()
        {
            onNotify.RemoveAllListeners();
        }
    }
}