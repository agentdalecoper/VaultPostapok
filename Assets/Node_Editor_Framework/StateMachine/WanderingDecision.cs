using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Wander")]
    public class WanderingDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.moveEntity.headingPlace == null && unitEntity.attackingUnitEntity == null)
            // {
            //     return true;
            // }
            //
            return false;
        }

        public override void OnSuccess(StateController controller)
        {
            // base.OnSuccess(controller);
            //
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // var rand = new Random();
            // var place = unitEntity.moveEntity.currentPlace.Neighors[rand.Next(unitEntity.moveEntity.currentPlace.Neighors.Count)];
            // unitEntity.moveEntity.GetComponent<MoveComponent>().SetCommandToMove(place.Center);
        }
    }
}