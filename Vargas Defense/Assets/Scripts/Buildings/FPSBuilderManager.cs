using System;
using System.Collections.Generic;
using UnityEngine;

public class FPSBuilderManager : MonoBehaviour {

	public GameObject placeablePrefab; //set from elsewhere
	public Transform buildSpawn;
	public float maxHeight = 0.5f;

	[SerializeField]
	private KeyCode newObjectHotkey = KeyCode.T;


	public Transform player;

	public bool isBuilding { get; private set;}
	public float currentResources = 0;

	private GameObject currentPlaceablePrefab;
	private Camera playerView;
	private float degreeAddition = 0;

	public static FPSBuilderManager Instance;
	public List<Buildable> builds;

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
	
	
	void Update () {
		HandleObjectKey();
		if(currentPlaceablePrefab != null){
			MoveCurrentPlaceable();
			//RotateCurrentPlaceable();
			ReleaseIfClicked();
		}
		else
		{
		//check clicks for repair/upgrade	
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
			if(currentPlaceablePrefab == null){
				currentPlaceablePrefab = Instantiate(placeablePrefab);
				currentPlaceablePrefab.SetActive(true);
				Buildable b = currentPlaceablePrefab.GetComponent<Buildable>();
				if(b) b.StartProjecting();	
				isBuilding =true;
			}else{
				degreeAddition =0;
				Destroy(currentPlaceablePrefab);//substutir depois quando pooler for implementado
				isBuilding = false;
			}
		}
    }

	public void InsertResources(){
		Ray ray = playerView.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo)){
			
		}
	}
}
