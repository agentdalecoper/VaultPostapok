using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/LookForEnemyDecision")]
    public class AttackDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // foreach (Place place in unitEntity.moveEntity.currentPlace.Neighors)
            // {
            //     UnitEntity otherUnitEntity = CheckEnemiesInPlace(place, unitEntity);
            //     if (otherUnitEntity != null)
            //     {
            //         unitEntity.attackingUnitEntity = otherUnitEntity;
            //         return true;
            //     }
            // }
            //
             return false;
        }

        // public static UnitEntity CheckEnemiesInPlace(Place place, UnitEntity unitEntity)
        // {
        //     // List<MonoBehaviour> monoBehaviours = PlaceController.Instance.GetMonoBehavioursInPlace(place);
        //     //
        //     // if (monoBehaviours == null) return null;
        //     // foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        //     // {
        //     //     if (monoBehaviour == null || !(monoBehaviour is MoveComponent)) continue;
        //     //
        //     //     UnitEntity otherUnitEntity = monoBehaviour.gameObject.GetComponent<UnitEntity>();
        //     //
        //     //     if (otherUnitEntity == null) continue;
        //     //     if (otherUnitEntity.unitSide == unitEntity.unitSide) continue;
        //     //
        //     //     return otherUnitEntity;
        //     // }
        //     //
        //     // return null;
        // }
    }
}