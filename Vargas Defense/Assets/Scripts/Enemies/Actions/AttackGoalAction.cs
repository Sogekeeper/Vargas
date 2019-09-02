using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/AttackGoal")]
public class AttackGoalAction : Action
{
    public override void Act(StateController controller)
    {
        TryToAttack(controller);
    }

    private void TryToAttack(StateController controller){
        if(controller.enemyStats.goal == null) return;
        float dist = Vector3.Distance(controller.trans.position, controller.enemyStats.goal.position);
        //Debug.Log(dist.ToString());
        if(dist < controller.enemyStats.attackGoalRange){
            controller.anim.SetBool("attacking", true);
        }
        else{
            controller.anim.SetBool("attacking", false);
        }
    }
}
