using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/ChasePlayer")]
public class ChasePlayerAction : Action
{
    public override void Act(StateController controller)
    {
        ChasePlayer(controller);
    }

    private void ChasePlayer(StateController controller){
        controller.agent.SetDestination(FPSBuilderManager.Instance.transform.position);
    }
}
