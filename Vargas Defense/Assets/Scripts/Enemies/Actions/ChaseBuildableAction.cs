using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/ChaseTower")]
public class ChaseBuildableAction : Action
{
    public override void Act(StateController controller){
        ChaseTower(controller);
    }

    private void ChaseTower(StateController controller){
        if(controller.enemyStats.target == null) return;
        controller.agent.SetDestination(controller.enemyStats.target.transform.position);
    }
}
