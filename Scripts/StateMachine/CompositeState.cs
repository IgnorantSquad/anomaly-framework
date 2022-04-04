using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Anomaly
{
    public class CompositeState : State
    {
        public override Identity ID => State.Identity.None;

        private List<State> states = new List<State>();

        public CompositeState(params State[] states)
        {
            this.states.AddRange(states);
        }


        public override void OnEnter(CustomBehaviour target)
        {
            for (int i = 0; i < states.Count; ++i)
            {
                states[i].OnEnter(target);
            }
        }

        public override void OnExit(CustomBehaviour target)
        {
            for (int i = 0; i < states.Count; ++i)
            {
                states[i].OnExit(target);
            }
        }

        public override void OnFixedUpdate(CustomBehaviour target)
        {
            for (int i = 0; i < states.Count; ++i)
            {
                states[i].OnFixedUpdate(target);
            }
        }

        public override void OnLateUpdate(CustomBehaviour target)
        {
            for (int i = 0; i < states.Count; ++i)
            {
                states[i].OnLateUpdate(target);
            }
        }

        public override void OnUpdate(CustomBehaviour target)
        {
            for (int i = 0; i < states.Count; ++i)
            {
                states[i].OnUpdate(target);
            }
        }
    }
}