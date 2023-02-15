using System.Collections.Generic;
using System;
using UnityEngine;

namespace Anomaly
{
    public class AUpdateManager : Anomaly.Utils.ASingletonBehaviour<AUpdateManager>
    {
        private List<(ABehaviour target, Action method)> fixedUpdateObjectList = new List<(ABehaviour, Action)>();
        private List<(ABehaviour target, Action method)> updateObjectList = new List<(ABehaviour, Action)>();
        private List<(ABehaviour target, Action method)> lateUpdateObjectList = new List<(ABehaviour, Action)>();

        public void RegisterFixedUpdate(ABehaviour target, System.Action action)
        {
            fixedUpdateObjectList.Add((target, action));
        }
        public void RegisterUpdate(ABehaviour target, System.Action action)
        {
            updateObjectList.Add((target, action));
        }
        public void RegisterLateUpdate(ABehaviour target, System.Action action)
        {
            lateUpdateObjectList.Add((target, action));
        }


        void FixedUpdate()
        {
            UpdateLoop(fixedUpdateObjectList);
        }

        void Update()
        {
            UpdateLoop(updateObjectList);
        }

        void LateUpdate()
        {
            UpdateLoop(lateUpdateObjectList);
        }

        private void UpdateLoop(List<(ABehaviour target, System.Action method)> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                var current = list[i];
                if (current.target == null)
                {
                    list.RemoveAt(i);
                    continue;
                }
                if (!current.target.gameObject.activeInHierarchy)
                {
                    continue;
                }
                current.method.Invoke();
            }
        }
    }

}