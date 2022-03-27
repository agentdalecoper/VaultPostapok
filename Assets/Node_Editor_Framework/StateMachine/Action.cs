namespace SquadSoldier.StateMachine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class Action : ScriptableObject
    {
        public virtual void OnEnter(StateController controller)
        {
        }
        
        public abstract void Act(StateController controller);

        public virtual void OnExit(StateController controller)
        {
        }
    }
}