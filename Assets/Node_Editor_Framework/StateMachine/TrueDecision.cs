using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/TrueDecision")]
    public class TrueDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            return true;
        }
    }
}