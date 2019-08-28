using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int totalLife = 20;
    public int currentLife{get; private set;}

    private void Start() {
        currentLife = totalLife;
    }

    public void TakeDamage(int dmg){
        currentLife -= dmg;
        if(currentLife <= 0){
            //Die()
            gameObject.SetActive(false);
        }
    }
}
