using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly.Utils
{
    public class APoolObject : ABehaviour
    {
        public enum State
        {
            Prepare,
            Using
        }

        public State CurrentState { get; set; } = State.Prepare;

        public string Name { get; set; }

        public void Return()
        {
            APoolManager.Instance.Add(this);
        }

    }
}