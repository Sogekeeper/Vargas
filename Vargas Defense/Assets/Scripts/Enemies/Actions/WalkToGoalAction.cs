using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/WalkToGoal")]
public class WalkToGoalAction : Action
{
    public override void Act(StateController controller){
        Walk(controller);
    }

    private void Walk(StateController controller){
        controller.agent.SetDestination(controller.enemyStats.goal.position);
    }
}
