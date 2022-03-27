using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/PlayerMove")]
    public class MoveAction : Action
    {
        public override void Act(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // MoveComponent moveComponent = unitEntity.GetComponent<MoveComponent>();
            // moveComponent.Act();
        }

        public override void OnExit(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // unitEntity.gameObject.transform.position = unitEntity.moveEntity.currentPlace.Center;
            // unitEntity.moveEntity.path = null;
            // unitEntity.moveEntity.headingPlace = null;
        }
    }
}