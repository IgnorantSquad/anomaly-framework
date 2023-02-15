using UnityEngine;
using System.Collections;

namespace Anomaly.Utils
{
    public class AStream
    {
        public class Data
        {
            public float interval;
            public int count;
        }

        public delegate void Event(Data data);


        public bool IsOpen { get; private set; }


        private MonoBehaviour attachedTarget;
        private Coroutine coroutine;

        private System.Func<bool> select;
        private System.Func<Data, bool> bind;
        private System.Func<Data, bool> notifyWhen;

        private Event callback;

        private Data collectedData = new Data();


        public static AStream Create(MonoBehaviour attachedBehaviour)
        {
            return new AStream()
            {
                attachedTarget = attachedBehaviour
            };
        }


        public AStream Select(System.Func<bool> select)
        {
            this.select = select;
            return this;
        }
        public AStream Bind(System.Func<Data, bool> bind)
        {
            this.bind = bind;
            return this;
        }
        public AStream NotifyWhen(System.Func<Data, bool> notifyWhen)
        {
            this.notifyWhen = notifyWhen;
            return this;
        }

        public AStream Subscribe(Event callback)
        {
            this.callback += callback;
            return this;
        }
        public void Unsubscribe(Event callback)
        {
            this.callback -= callback;
        }


        public void Open()
        {
            IsOpen = true;
        }
        public void Close()
        {
            IsOpen = false;
            collectedData.interval = 0F;
            collectedData.count = 0;
        }


        private void Validate()
        {
            Debug.Assert(select != null, message: "Call Select() first");
            Debug.Assert(attachedTarget != null, message: "Attach any behaviour");
        }


        public void Start()
        {
            Validate();
            Open();
            coroutine = attachedTarget.StartCoroutine(CoUpdate());

            IEnumerator CoUpdate()
            {
                while (true)
                {
                    Update();
                    yield return null;
                }
            }
        }

        private void Update()
        {
            if (!IsOpen) return;

            collectedData.interval += Time.deltaTime;

            if (!select.Invoke()) return;

            var shouldBind = bind != null && bind.Invoke(collectedData);

            collectedData.count = shouldBind ? collectedData.count + 1 : 1;

            var notify = notifyWhen == null ? true : notifyWhen.Invoke(collectedData);
            if (notify)
            {
                callback?.Invoke(collectedData);

                collectedData.count = 0;
            }

            collectedData.interval = 0F;
        }
    }
}