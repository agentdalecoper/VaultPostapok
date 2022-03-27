using NodeEditorFramework;

namespace SquadSoldier.StateMachine
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "PluggableAI/State")]
    public class State : ScriptableObject
    {
        public Action[] actions;
        public Transition[] transitions;

        public void UpdateState(StateController controller)
        {
            CheckTransitions(controller);
            DoActions(controller);
        }

        private void DoActions(StateController controller)
        {
            foreach (Action action in actions)
            {
                action.Act(controller);
            }
        }

        private void CheckTransitions(StateController controller)
        {
            foreach (var transition in transitions)
            {
                Decision decision = transition.decision;
                bool decisionSucceeded = decision.Decide(controller);
                if (!decisionSucceeded) continue;
                
                decision.OnSuccess(controller);
                controller.TransitionToState(transition.trueState);
                break;
            }
        }
    }
}