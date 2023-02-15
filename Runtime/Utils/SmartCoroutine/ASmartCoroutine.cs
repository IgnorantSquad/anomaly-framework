using System.Collections;
using UnityEngine;

namespace Anomaly.Utils
{
    public partial class ASmartCoroutine
    {
        protected static ASmartCoroutineInstance instance = null;


        private Coroutine coroutine;

        public bool IsRunning { get; protected set; }

        public System.Func<IEnumerator> Body { protected get; set; }

        public delegate void SmartCoroutineDelegate();
        public SmartCoroutineDelegate onFinished, onAborted;


        public ASmartCoroutine()
        {
            if (instance == null)
            {
                instance = new GameObject("[SmartCoroutine]").AddComponent<ASmartCoroutineInstance>();
                GameObject.DontDestroyOnLoad(instance.gameObject);
            }
        }

        public static ASmartCoroutine Create(System.Func<IEnumerator> body = null)
        {
            return new ASmartCoroutine() { Body = body };
        }

        public static ASmartCoroutine Create(IEnumerator body)
        {
            return new ASmartCoroutine().StartImmediate(body);
        }

        public ASmartCoroutine OnFinished(SmartCoroutineDelegate callback)
        {
            onFinished += callback;
            return this;
        }
        public ASmartCoroutine OnAborted(SmartCoroutineDelegate callback)
        {
            onAborted += callback;
            return this;
        }


        public virtual ASmartCoroutine Start()
        {
            if (Body == null) throw new ANoCoroutineBodyException();

            Stop();
            coroutine = instance.StartCoroutine(CoStart());

            return this;

            IEnumerator CoStart()
            {
                IsRunning = true;

                yield return Body();

                // Aborted
                if (!IsRunning) yield break;

                IsRunning = false;
                onFinished?.Invoke();
            }
        }
        public ASmartCoroutine StartImmediate(IEnumerator body)
        {
            Stop();
            coroutine = instance.StartCoroutine(CoStart());

            return this;

            IEnumerator CoStart()
            {
                IsRunning = true;

                yield return body;

                // Aborted
                if (!IsRunning) yield break;

                IsRunning = false;
                onFinished?.Invoke();
            }
        }

        public virtual void Stop()
        {
            if (coroutine == null) return;
            if (!IsRunning) return;

            instance.StopCoroutine(coroutine);

            IsRunning = false;
            onAborted?.Invoke();
        }
    }
}