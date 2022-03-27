using System;
using DefaultNamespace;

namespace SquadSoldier.StateMachine
{
    using UnityEngine;

    //ToDo the graf start building from the stateController
    //ToDo Command pattern history rollback
    public class StateController : MonoBehaviour
    {
        public State currentState;

        [HideInInspector] public float stateTimeElapsed;

        // public UnitEntity unitEntity;
        //
        // private void Awake()
        // {
        //     unitEntity = GetComponent<UnitEntity>();
        // }


//        void Update()
//        {
//            if (!gameObject.activeSelf)
//                return;
//            currentState.UpdateState(this);
//        }

        public void TransitionToState(State nextState)
        {
            if (nextState == currentState)
            {
                return;
            }

            foreach (Action action in currentState.actions)
            {
                action.OnExit(this);
            }

            stateTimeElapsed = 0f;
            currentState = nextState;

            foreach (Action action in nextState.actions)
            {
                action.OnEnter(this);
            }
        }

        public bool CheckIfCountDownElapsed(float duration)
        {
            stateTimeElapsed += Time.deltaTime;
            if (stateTimeElapsed >= duration)
            {
                stateTimeElapsed = 0f;
                return true;
            }

            return false;
        }
    }
}