using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/SimpleAttack")]
public class AttackSimpleAction : Action
{
    public override void Act(StateController controller)
    {
        Attack(controller);
    }

    private void Attack(StateController controller){
        controller.anim.SetBool("attacking", true);
    }
}
