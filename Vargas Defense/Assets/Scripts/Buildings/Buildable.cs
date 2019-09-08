using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Buildable : MonoBehaviour
{
    [System.Serializable]
    public class Upgrade{
        public int cost = 25;
        public GameObject targetBuild; //link on inspector
    }

    public int totalLife = 20;
    public int cost = 10;
    [TextArea]
    public string description = "Descrição do que a torre é ou faz.";
    public float timeToBuild = 20f;    
    public float multiplierDuration = 1.5f;
    public bool isTower = true; //pro inimigo bate
    public GameObject placementProjection;
    public GameObject initialBuild;

    [HideInInspector] public int threats = 0;

    [Header("Upgrades")]
    public Upgrade[] upgrades;

    [Header("Optional Visuals")]
    public ParticleSystem damageParticles;
    public GameObject buildingProgressInterface;
    public Image buildingProgressBar;
    public TextMeshProUGUI buildingProgressText;
    public GameObject buildingLifeInterface;
    public Image buildingLifeBar;
    public TextMeshProUGUI buildingLifeText;

    public int currentLife {get; protected set;}
    public int currentUpgradeProgress {get; protected set;}
    public int currentUpgradeIndex {get; protected set;}
    
    public float progress {get; protected set;}
    public bool isBuilding{get; protected set;}

    private float multiplierTimer = 0;

    public void StartProjecting(){
        isBuilding = false;
        if(buildingProgressInterface) buildingProgressInterface.SetActive(false);
        initialBuild.SetActive(false);
        placementProjection.SetActive(true);
        if(upgrades != null && upgrades.Length >= 1){
            for(int i = 0; i < upgrades.Length; i++){
                upgrades[i].targetBuild.SetActive(false);
            }
        }
        if(buildingProgressInterface)buildingProgressInterface.SetActive(false);
        if(buildingLifeInterface)buildingLifeInterface.SetActive(false);
        currentUpgradeIndex = 0;
        currentUpgradeProgress = 0;

    }

    public void StartBuilding(){
        isBuilding = true;
        if(buildingProgressInterface) buildingProgressInterface.SetActive(true);
        initialBuild.SetActive(true);
        placementProjection.SetActive(false);
        IBuildBehaviour bb = initialBuild.GetComponent<IBuildBehaviour>();
        if(bb != null)bb.StartBuilding();
        progress = 0;
        threats = 0;
        if(buildingProgressInterface)buildingProgressInterface.SetActive(true);
        if(buildingLifeInterface)buildingLifeInterface.SetActive(false);
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
        if(buildingProgressBar) buildingProgressBar.fillAmount = progress/timeToBuild;
        if(progress >= timeToBuild){
            IBuildBehaviour bb = initialBuild.GetComponent<IBuildBehaviour>();
            if(bb != null)bb.FinishedBuilding();
            if(buildingProgressInterface) buildingProgressInterface.SetActive(false);
            if(buildingLifeInterface)buildingLifeInterface.SetActive(true);
            if(buildingLifeBar) buildingLifeBar.fillAmount = (float)currentLife/(float)totalLife;
            isBuilding = false;
        }
    }

    public void TakeDamage(int dmg){
        currentLife -= dmg;
        if(buildingLifeBar) buildingLifeBar.fillAmount = (float)currentLife/(float)totalLife;
        if(damageParticles){
            damageParticles.Stop();
            damageParticles.Play();
        }
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
    public int GetNeededAmount(ref bool isUpgrade){
        int amountNeeded = 0;
        FPSBuilderManager builder = FPSBuilderManager.Instance;
        if(currentLife < totalLife){
            amountNeeded = totalLife - currentLife;
            if(amountNeeded > builder.maxRepairAmount) amountNeeded = builder.maxRepairAmount;
            if(builder.currentResources < amountNeeded) amountNeeded = builder.currentResources;
            isUpgrade = false;
            
        }else if(upgrades != null && upgrades.Length > 0){
            amountNeeded = upgrades[currentUpgradeIndex].cost - currentUpgradeProgress;
            if(amountNeeded > builder.maxRepairAmount) amountNeeded = builder.maxRepairAmount;
            if(builder.currentResources < amountNeeded) amountNeeded = builder.currentResources;
            isUpgrade = true;
        }
        return amountNeeded;
    } 
}
