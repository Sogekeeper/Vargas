using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/DeadPlayer")]
public class DeadPlayerDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool playerDead = CheckPlayerLife(controller);
        return playerDead;
    }

    private bool CheckPlayerLife(StateController controller)
    {
        if(FPSBuilderManager.Instance.playerStats.currentLife <= 0)
            return true;
        else
            return false;
    }
}
