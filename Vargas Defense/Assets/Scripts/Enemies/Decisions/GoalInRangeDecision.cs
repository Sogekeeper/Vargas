using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/GoalInRange")]
public class GoalInRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool nextToGoal = CheckGoal(controller);
        return nextToGoal;
    }

    private bool CheckGoal(StateController controller){
        if(controller.enemyStats.goal == null) return false;
        float dist = Vector3.Distance(controller.trans.position, controller.enemyStats.goal.position);
        //Debug.Log(dist.ToString());
        if(dist < controller.enemyStats.attackGoalRange){
            return true;
        }
        else{
            return false;
        }
    }
}
