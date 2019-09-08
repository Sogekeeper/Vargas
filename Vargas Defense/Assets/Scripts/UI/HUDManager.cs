using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    [Header("Permanent HUD - All Optional")]
    public TextMeshProUGUI resourcesLabel;
    public TextMeshProUGUI playerLifeLabel;
    public Image playerLifeBar;
    public TextMeshProUGUI goalLifeLabel;
    public Image goalLifeBar;
    public Image weaponCooldownBar;
    public TextMeshProUGUI weaponCooldownText;
    [Header("Contextual Buildable HUD")]
    public GameObject towerInterface;
    public GameObject speedBuildInterface;
    public TextMeshProUGUI towerRepairTextHeadline;
    public TextMeshProUGUI towerRepairTextCost;

    [Header("References from Entities")]
    public PlayerStats player;
    public Buildable goal;
    public FPSBuilderManager builder;
    private Camera playerView;

    private void Start() {
        playerView = Camera.main;
        towerInterface.SetActive(false);
        speedBuildInterface.SetActive(false);
        StartCoroutine(CheckForBuildable());
        StartCoroutine(UpdateTextAndBars());
    }

    private void Update() {
        //usar depois para fazer barras de trás lentamente seguir as da frente
    }

    private IEnumerator UpdateTextAndBars(){
        while(true){
            ////////REMOVER OS IFS E VARS DESNECESSARIAS NA BUILD FINAL
            if(resourcesLabel)resourcesLabel.text = builder.currentResources.ToString();
            if(playerLifeLabel)playerLifeLabel.text = player.currentLife.ToString();
            if(goalLifeLabel)goalLifeLabel.text = goal.currentLife.ToString();

            if(playerLifeBar) playerLifeBar.fillAmount = (float)player.currentLife/(float)player.totalLife;
            if(goalLifeBar) goalLifeBar.fillAmount = (float)goal.currentLife/(float)goal.totalLife;      

            if(weaponCooldownBar) weaponCooldownBar.fillAmount = player.weaponTimer / player.weaponCooldown;
            float secondsLeft = player.weaponCooldown - player.weaponTimer;
            if(weaponCooldownText){
                if(secondsLeft <= 0) weaponCooldownText.text = "PRONTO";
                else weaponCooldownText.text = Mathf.Ceil(secondsLeft).ToString();
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator CheckForBuildable()
    {
        while(true){
            Ray ray = playerView.ScreenPointToRay(Input.mousePosition);
		
		    RaycastHit hitInfo;
		    if(Physics.Raycast(ray, out hitInfo,builder.repairRange) && !builder.placementMenu.activeInHierarchy){			
                Buildable b = hitInfo.collider.GetComponent<Buildable>();
                if(b && b.progress >= b.timeToBuild){
                    towerInterface.SetActive(true);
                    speedBuildInterface.SetActive(false);
                    bool isUpgrade = false;   
                    int amount = b.GetNeededAmount(ref isUpgrade); 
                    if(!isUpgrade)towerRepairTextHeadline.text = "Reparar torre por:";
                    else towerRepairTextHeadline.text = "Aprimorar torre por:";
                    towerRepairTextCost.text = amount.ToString();
                    if(amount == 0) towerRepairTextHeadline.text = "Nada para aprimorar";
                }else if(b && b.isBuilding){
                    towerInterface.SetActive(false);    
                    speedBuildInterface.SetActive(true);                    
                }else{
                    towerInterface.SetActive(false);    
                    speedBuildInterface.SetActive(false);
                }
            		
		    }else{
                towerInterface.SetActive(false);
                speedBuildInterface.SetActive(false);
            }

            yield return new WaitForSeconds(0.15f);
        }
    }
}
