using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/PlayerInRange")]
public class PlayerInRangeDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool playerInRange = LookForPlayer(controller);
        return playerInRange;
    }

    private bool LookForPlayer(StateController controller){
        float playerDistFromGoal = Vector3.Distance(FPSBuilderManager.Instance.transform.position, controller.enemyStats.goal.position);
        float playerDistFromYou  = Vector3.Distance(FPSBuilderManager.Instance.transform.position, controller.trans.position);
        float yourDistFromGoal   = Vector3.Distance(controller.enemyStats.goal.position, controller.trans.position);
        if(playerDistFromGoal-1 < yourDistFromGoal && playerDistFromYou <  controller.enemyStats.playerRange){
            controller.enemyStats.attackingPlayer = true;
            return true;
        }else
        {
            controller.enemyStats.attackingPlayer = false;
            return false;
        }
    }
}
