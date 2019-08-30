using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBuild : Buildable
{
    
    void Start()
    {
        isBuilding = false;    
        currentLife = totalLife;
    }

    
}
