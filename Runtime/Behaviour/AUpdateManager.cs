using System;
using System.Collections.Generic;

namespace Anomaly
{
    public class AUpdateManager : Anomaly.Utils.ASingletonBehaviour<AUpdateManager>
    {
        public class Data
        {
            public ABehaviour target;
            public Action method;
            public Data next = null;
        }

        private Data fixedUpdateObjects = null;
        private Data updateObjects = null;
        private Data lateUpdateObjects = null;


        public void RegisterFixedUpdate(Data data)
        {
            data.next = fixedUpdateObjects;
            fixedUpdateObjects = data;
        }

        public void RegisterUpdate(Data data)
        {
            data.next = updateObjects;
            updateObjects = data;
        }

        public void RegisterLateUpdate(Data data)
        {
            data.next = lateUpdateObjects;
            lateUpdateObjects = data;
        }


        void FixedUpdate()
        {
            Data parent = null;
            var current = fixedUpdateObjects;

            while (current != null)
            {
                if (current.target == null || !current.target.gameObject.activeInHierarchy)
                {
                    if (parent == null)
                    {
                        fixedUpdateObjects = current.next;
                        current.next = null;
                        current = fixedUpdateObjects;
                        continue;
                    }

                    parent.next = current.next;
                    current.next = null;
                    current = parent.next;
                }

                current.method.Invoke();

                parent = current;
                current = current.next;
            }
        }

        void Update()
        {
            Data parent = null;
            var current = updateObjects;

            while (current != null)
            {
                if (current.target == null || !current.target.gameObject.activeInHierarchy)
                {
                    if (parent == null)
                    {
                        updateObjects = current.next;
                        current.next = null;
                        current = updateObjects;
                        continue;
                    }

                    parent.next = current.next;
                    current.next = null;
                    current = parent.next;
                }

                current.method.Invoke();

                parent = current;
                current = current.next;
            }
        }

        void LateUpdate()
        {
            Data parent = null;
            var current = lateUpdateObjects;

            while (current != null)
            {
                if (current.target == null || !current.target.gameObject.activeInHierarchy)
                {
                    if (parent == null)
                    {
                        lateUpdateObjects = current.next;
                        current.next = null;
                        current = lateUpdateObjects;
                        continue;
                    }

                    parent.next = current.next;
                    current.next = null;
                    current = parent.next;
                }

                current.method.Invoke();

                parent = current;
                current = current.next;
            }
        }
    }
}