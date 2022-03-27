using System;
// using Components;
using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Actions/WanderAction")]
    public class WanderAction : Action
    {
        public override void Act(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // var place = unitEntity.moveEntity.currentPlace.Neighors.GetRandom();
            // unitEntity.moveEntity.GetComponent<MoveComponent>().SetCommandToMove(place.Center);
        }
    }
}