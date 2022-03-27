using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/ChaseDecision")]
    public class ChaseDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.lastSeenEnemies.Last() != null)
            // {
            //     List<Place> path
            //         = PlaceController.Instance.FindPath(unitEntity.moveEntity.currentPlace, unitEntity.lastSeenEnemies.Last().moveEntity.currentPlace);
            //
            //     if (path.Count > 0 && path.Count <= 2)
            //     {
            //         //ToDo move assigment out from decision
            //         unitEntity.moveEntity.GetComponent<MoveComponent>().SetCommandToMove(path.Last().Center);
            //         return true;
            //     }
            // }

            return false;
        }
    }
}