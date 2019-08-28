using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decisions/TurretInRange")]
public class TurretInRangeDecision : Decision
{
    public override bool Decide(StateController controller){
        bool turretInRange = LookForTurret(controller);
        return turretInRange;
    }

    private bool LookForTurret(StateController controller){        
        for (int i = 0; i < FPSBuilderManager.Instance.builds.Count; i++)
        {
            Buildable b = FPSBuilderManager.Instance.builds[i];
            float distance = Vector3.Distance(controller.trans.position, b.transform.position);
            if(b.isTower && !b.isBuilding && distance <= controller.enemyStats.turretRange && b.threats < controller.enemyStats.ignoreValue){
                controller.enemyStats.target = b;
                b.threats += controller.enemyStats.threatValue;
                Debug.Log("found turret");
                return true;                
            }
        }
        return false;
    }
}
