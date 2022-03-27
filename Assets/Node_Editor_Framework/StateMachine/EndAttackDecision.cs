using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/EndAttackDecisionTest")]
    public class EndAttackDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.attackingUnitEntity == null
            //     || !unitEntity.moveEntity.currentPlace.neighbors.Contains(unitEntity.attackingUnitEntity.moveEntity.currentPlace))
            // {
            //     Debug.Log("End attack");
            //     return true;
            // }

            return false;
        }
    }
}