using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/DestroyedBuild")]
public class DestroyedBuildDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool buildIsDetroyed = CheckTarget(controller);
        return buildIsDetroyed;
    }

    private bool CheckTarget(StateController controller){
        //if(controller.enemyStats.target == null) return true;
        if(controller.enemyStats.target.currentLife <= 0 ){
            controller.anim.SetBool("attacking", false);
            return true;
        }else{
            return false;
        }
    }
}
