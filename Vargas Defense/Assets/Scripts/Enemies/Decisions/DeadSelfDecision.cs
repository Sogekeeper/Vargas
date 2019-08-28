using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/DeadSelf")]
public class DeadSelfDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool checkSelf = ImDead(controller);
        return checkSelf;
    }

    private bool ImDead(StateController controller){
        if(controller.enemyStats.currentLife <= 0){
            return true;
        }else{
            return false;
        }
    }
}
