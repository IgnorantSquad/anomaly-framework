using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public abstract class State
    {
        public enum Identity
        {
            None,
            PlayerLocomotion,
            PlayerInteraction,
            PlayerAttack
        }

        public abstract Identity ID { get; }

        public abstract void OnEnter(CustomBehaviour target);
        public abstract void OnExit(CustomBehaviour target);
        public abstract void OnFixedUpdate(CustomBehaviour target);
        public abstract void OnUpdate(CustomBehaviour target);
        public abstract void OnLateUpdate(CustomBehaviour target);

        public static State New<T>() where T : State, new()
        {
            return new T();
        }

        public static State Bind(params State[] states)
        {
            return new CompositeState(states);
        }
    }
}