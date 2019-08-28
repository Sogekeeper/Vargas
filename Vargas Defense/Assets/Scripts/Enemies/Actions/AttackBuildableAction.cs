using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/AttackTower")]
public class AttackBuildableAction : Action
{
    public override void Act(StateController controller)
    {
        TryToAttack(controller);
    }

    private void TryToAttack(StateController controller){
        if(controller.enemyStats.target == null) return;
        float dist = Vector3.Distance(controller.trans.position, controller.enemyStats.target.transform.position);
        //Debug.Log(dist.ToString());
        if(dist < controller.enemyStats.attackTowerRange){
            controller.anim.SetBool("attacking", true);
        }
        else{
            controller.anim.SetBool("attacking", false);
        }
    }
}
