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
        Vector3 dirToTower = controller.enemyStats.target.transform.position - controller.trans.position;
        dirToTower.Normalize(); 
        Quaternion toRot = Quaternion.LookRotation(dirToTower, Vector3.up);
        controller.trans.rotation = Quaternion.Slerp(controller.trans.rotation, Quaternion.Euler(0,toRot.eulerAngles.y,0), 0.5f );
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
