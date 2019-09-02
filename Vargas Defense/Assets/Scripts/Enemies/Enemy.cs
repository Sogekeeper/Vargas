using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int totalLife = 10;
    public int currentLife{get; private set;}
    public int damage = 1;
    public int minDropValue = 3;
    public int maxDropValue = 6;
    [Space()]
    public float offsetY = 0.5f;
    [Space()]
    public float turretRange = 6f;
    public float playerRange = 10f;
    public float attackMeleeArea = 1f;
    public float attackMeleeRange = 20f;
    public float attackTowerRange = 2f;
    public float attackPlayerRange = 2f;
    public float attackGoalRange = 3f;
    [Space()]
    public int threatValue = 1;
    public int ignoreValue = 2;
    public LayerMask buildsLayer;
    [Space()]

    [HideInInspector]public bool attackingPlayer = false;

    public Transform goal;
    public Buildable target;
    [Header("Optional Interface")]
    public Image lifeBar;

    private bool alreadyDropped = false;

    private void Start() {
        Initialize();
    }

    public void Initialize(){//pra depois caso eu faça uma pool de inimigos eu ainda consiga chamar isso sem OnEnable
        currentLife = totalLife;
        alreadyDropped = false;
        if(lifeBar) lifeBar.fillAmount = (float)currentLife/(float)totalLife;
    }

    public void TakeDamage(int dmg){
        if(currentLife>0){
            currentLife -= dmg;
            if(lifeBar) lifeBar.fillAmount = (float)currentLife/(float)totalLife;
        }
        if(currentLife <= 0){
            if(!alreadyDropped){
                DropDatabase.Instance.DropAmount(Random.Range(minDropValue, maxDropValue), transform.position + new Vector3(0, offsetY,0));  
                alreadyDropped = true;
            } 
        }
    }

    public void DieEnd(){
        gameObject.SetActive(false);
        if(target!= null && target.currentLife > 0)target.threats -= threatValue;
    }

    private void Update() {
        //Debug.DrawRay(transform.position + new Vector3(0,1,0), transform.forward.normalized *attackMeleeRange , Color.green);
    }

    public virtual void Attack(){
        RaycastHit hit;
        
        if(!attackingPlayer){  
                                  
            if(Physics.SphereCast(transform.position + new Vector3(0,1,0),attackMeleeArea,transform.forward, out hit,attackMeleeRange,buildsLayer.value)){
                //Debug.Log(hit.collider.name);
                if(hit.collider != null){
                    Buildable b = hit.collider.gameObject.GetComponent<Buildable>();
                    if(b != null) b.TakeDamage(damage);
                }
            }
        }else{
            if(Physics.SphereCast(transform.position,attackMeleeArea,transform.forward, out hit,attackMeleeRange,LayerMask.NameToLayer("Player"))){

            }
        }
    }
}
