using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int totalLife;
    public int currentLife{get; private set;}
    public float turretRange = 6f;
    public float playerRange = 10f;
    public int threatValue = 1;
    public int ignoreValue = 2;

    public Transform goal;
    [HideInInspector] public Buildable target;

    private void Start() {
        Initialize();
    }

    public void Initialize(){//pra depois caso eu faça uma pool de inimigos eu ainda consiga chamar isso sem OnEnable
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
