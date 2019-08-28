using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade{
        public int cost = 25;
        public GameObject targetBuild; //link on inspector
    }

    public int totalLife = 20;
    public int cost = 10;
    public float timeToBuild = 20f;    
    public float multiplierDuration = 1.5f;
    public bool isTower = true; //pro inimigo bate
    public GameObject placementProjection;
    public GameObject initialBuild;

    [HideInInspector] public int threats = 0;

    [Header("Upgrades")]
    public Upgrade[] upgrades;

    public int currentLife ;
    public int currentUpgradeProgress {get; private set;}
    public int currentUpgradeIndex {get; private set;}
    
    public float progress {get; private set;}
    public bool isBuilding{get; private set;}

    private float multiplierTimer = 0;

    public void StartProjecting(){
        isBuilding = false;
        initialBuild.SetActive(false);
        placementProjection.SetActive(true);
        if(upgrades != null && upgrades.Length >= 1){
            for(int i = 0; i < upgrades.Length; i++){
                upgrades[i].targetBuild.SetActive(false);
            }
        }
        currentUpgradeIndex = 0;
        currentUpgradeProgress = 0;

    }

    public void StartBuilding(){
        isBuilding = true;
        initialBuild.SetActive(true);
        placementProjection.SetActive(false);
        IBuildBehaviour bb = initialBuild.GetComponent<IBuildBehaviour>();
        if(bb != null)bb.StartBuilding();
        progress = 0;
    }

    public void BoostBuilding(){
        multiplierTimer = 1f;
    }

    private void Update() {
        if(progress < timeToBuild && isBuilding){
            ProgressInitialBuild();
        }
        if(multiplierTimer > 0) multiplierTimer -= Time.deltaTime;
        
    }

    private void ProgressInitialBuild(){
        progress += multiplierTimer > 0 ? Time.deltaTime * multiplierDuration : Time.deltaTime * 1;
        currentLife = (int)(progress/timeToBuild * totalLife);
        if(progress >= timeToBuild){
            IBuildBehaviour bb = initialBuild.GetComponent<IBuildBehaviour>();
            if(bb != null)bb.FinishedBuilding();
            isBuilding = false;
        }
    }

    public void TakeDamage(int dmg){
        currentLife -= dmg;
        if(currentLife <= 0){
            Die();
        }
    }

    public void Die(){
        FPSBuilderManager.Instance.builds.Remove(this);
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public void InsertResource(ref int amount, int maxAmount){        
        int amountNeeded = 0;
        if(currentLife < totalLife){
            amountNeeded = totalLife - currentLife;
            if(amountNeeded > maxAmount) amountNeeded = maxAmount;
            if(amount < amountNeeded) amountNeeded = amount;
            currentLife += amountNeeded;
        }else if(upgrades != null && upgrades.Length > 0){
            amountNeeded = upgrades[currentUpgradeIndex].cost - currentUpgradeProgress;
            if(amountNeeded > maxAmount) amountNeeded = maxAmount;
            if(amount < amountNeeded) amountNeeded = amount;
            currentUpgradeProgress += amountNeeded;
        }
        print("Vida: "+currentLife.ToString()+"  Upgrade Progress: "+currentUpgradeProgress.ToString());
    }
    
}
