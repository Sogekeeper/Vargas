using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSBuilderManager : MonoBehaviour {

	[SerializeField]
	private KeyCode newObjectHotkey = KeyCode.T;

	[Header("Repair Settings")]
    public int maxRepairAmount = 5;
	public int currentResources = 0;
	public float repairRange = 2f;
	public bool isBuilding { get; private set;}
	
	public LayerMask buildsLayer; //pq eu não sei a performance de NameToLayer

	//Placing Settings
	[Header("Placement Settings")]
	public Transform buildSpawn;
	public float maxHeight = 0.5f;

	private GameObject currentPlaceablePrefab;
	private Camera playerView;
	private float degreeAddition = 0;

	[HideInInspector]public List<Buildable> builds;

	[Header("Player")]
	public Transform player;
	public PlayerStats playerStats;
	public Animator anim;

	[Header("Interface")]
	public GameObject placementMenu;	
	public CPMPlayer playerMov;

	//SINGLETON
	public static FPSBuilderManager Instance;

    void Awake () {
		if(Instance == null){
			Instance = this;
		}else{
			Destroy(gameObject);
			return;
		}
		isBuilding = false;
		builds  = new List<Buildable>();
		playerView = Camera.main;		
	}
	
	private void Start() {
		placementMenu.SetActive(false);		
	}
	
	
	void Update () {
		HandleObjectKey();
		if(currentPlaceablePrefab != null){ //Está com o holograma pra colocar build
			MoveCurrentPlaceable();
			//RotateCurrentPlaceable();
			ReleaseIfClicked();
		}
		else if(!placementMenu.activeInHierarchy) //pode atirar e reparar coisas
		{
			HandleTools();	
		}

	}

	private void HandleTools(){
		if(Input.GetButtonDown("Fire1")){
			playerStats.Fire();
		}
		if(Input.GetButtonDown("Fire2")){
			InsertResources();
		}
	}
	

    private void ReleaseIfClicked()
    {
		if(Input.GetButtonDown("Fire1") && currentPlaceablePrefab.activeInHierarchy){
			Buildable b = currentPlaceablePrefab.GetComponent<Buildable>();
			if(b) b.StartBuilding();
			builds.Add(b);
			currentResources -= b.cost;			
			currentPlaceablePrefab = null;
			isBuilding = false;
			degreeAddition =0;
		}
		if(Input.GetButtonDown("Fire2")){
			degreeAddition += 90;
		}
    }


    private void MoveCurrentPlaceable()
    {
        Ray ray = new Ray(buildSpawn.position, Vector3.down);
		//Ray ray = playerView.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hitInfo;	

		if(Physics.Raycast(ray, out hitInfo, 3)){
			currentPlaceablePrefab.transform.position = hitInfo.point;
			//currentPlaceablePrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			currentPlaceablePrefab.transform.rotation = Quaternion.Euler(0, player.transform.rotation.eulerAngles.y + degreeAddition,0);
		}

		if(hitInfo.distance > maxHeight){
			currentPlaceablePrefab.SetActive(false);
		}
		else{
			currentPlaceablePrefab.SetActive(true);
		}
    }

    private void HandleObjectKey()
    {
        if(Input.GetKeyDown(newObjectHotkey)){
			if(currentPlaceablePrefab == null && !placementMenu.activeInHierarchy){
				playerMov.ToggleCamera(false);
				placementMenu.SetActive(true);
			}
			else if(placementMenu.activeInHierarchy){
				playerMov.ToggleCamera(true);
				placementMenu.SetActive(false);
			}
			else{
				degreeAddition =0;
				Destroy(currentPlaceablePrefab);//substutir depois quando pooler for implementado
				isBuilding = false;
			}
		}
    }

	public void HandleBuildSelection(int targetBuildIndex){		
		Buildable b = BuildingsDatabase.Instance.GetBuildable(targetBuildIndex);
		if(b.cost > currentResources){
			//Go Wrong
			
			b.gameObject.SetActive(false);
			return;
		}
		currentPlaceablePrefab = b.gameObject;
		currentPlaceablePrefab.SetActive(true);
		if(b) b.StartProjecting();	
		isBuilding =true;
		placementMenu.SetActive(false);
		playerMov.ToggleCamera(true);
	}

	public void InsertResources(){
		Ray ray = playerView.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo,repairRange,buildsLayer.value)){
			Buildable b = hitInfo.collider.GetComponent<Buildable>();
			if(b.isBuilding) b.BoostBuilding();
			else b.InsertResource(ref currentResources, maxRepairAmount);			
		}
	}
}
