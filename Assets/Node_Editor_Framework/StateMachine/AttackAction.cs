// using Components;
using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
    public class AttackAction : Action
    {
        public override void Act(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // Attack(unitEntity);
        }
        //
        // public override void OnExit(StateController controller)
        // {
        //     base.OnExit(controller);
        //
        //     UnitEntity unitEntity = controller.unitEntity;
        //
        //     unitEntity.lastSeenEnemies.Add(unitEntity.attackingUnitEntity);
        //     unitEntity.attackingUnitEntity = null;
        // }
        //
        // //ToDo move it to the separate component
        // void Attack(UnitEntity unitEntity)
        // {
        //     UnitController.Instance.AttackEnemy(unitEntity, unitEntity.attackingUnitEntity);
        // }
    }
}