using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/PlayerMoveDecision")]
    public class MoveDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.moveEntity.headingPlace != null)
            // {
            //     return true;
            // }

            return false;
        }
    }
}