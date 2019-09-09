using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Actions/Die")]
public class DieAction : Action
{
    public override void Act(StateController controller)
    {
        Die(controller);
    }

    private void Die(StateController controller){
        controller.anim.SetBool("dead", true);
        controller.agent.updatePosition = false;
    }
}
