using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/EndMoveDecision")]
    public class EndMoveDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.moveEntity.headingPlace == null || PlaceController.Instance.HasUnitsInPlace(unitEntity.moveEntity.headingPlace))
            // {
            //     return true;
            // }

            return false;
        }
    }
}