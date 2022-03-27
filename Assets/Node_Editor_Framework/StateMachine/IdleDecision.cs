using DefaultNamespace;
using UnityEngine;

namespace SquadSoldier.StateMachine
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/IdleDecision")]
    public class IdleDecision : Decision
    {
        public override bool Decide(StateController controller)
        {
            // UnitEntity unitEntity = controller.unitEntity;
            //
            // if (unitEntity.moveEntity.headingPlace == null)
            // {
            //     return true;
            // }

            return false;
        }
    }
}