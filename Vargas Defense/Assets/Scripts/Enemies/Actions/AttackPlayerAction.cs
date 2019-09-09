using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/AttackPlayer")]
public class AttackPlayerAction : Action
{
    public override void Act(StateController controller)
    {
        TryAttackingPlayer(controller);
    }

    private void TryAttackingPlayer(StateController controller){       
        Vector3 dirToPlayer = FPSBuilderManager.Instance.transform.position - controller.trans.position;
        dirToPlayer.Normalize(); 
        Quaternion toRot = Quaternion.LookRotation(dirToPlayer, Vector3.up);
        controller.trans.rotation = Quaternion.Slerp(controller.trans.rotation, Quaternion.Euler(0,toRot.eulerAngles.y,0), 0.5f );
        float dist = Vector3.Distance(controller.trans.position, FPSBuilderManager.Instance.transform.position);
        //Debug.Log(dist.ToString());
        if(dist < controller.enemyStats.attackPlayerRange){
            controller.anim.SetBool("attacking", true);
            controller.enemyStats.attackingPlayer = true;
        }
        else{
            controller.anim.SetBool("attacking", false);
            controller.enemyStats.attackingPlayer = false;
        }
    }
}
