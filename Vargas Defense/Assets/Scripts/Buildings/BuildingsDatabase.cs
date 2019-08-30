using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsDatabase : MonoBehaviour
{
    [System.Serializable]
    public class Pool{
        public Buildable buildPrefab;
        public int amount = 4;
    }

    [Header("Buildings")]
    public Pool[] buildables;    
    private Queue<Buildable>[] buildPools;

    [Header("Interface")]
    public RMF_RadialMenu radialMenu;
    public FPSBuilderManager builder;

    public static BuildingsDatabase Instance;     

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        if(buildables == null || buildables.Length <= 0) return;

        /////GENERATE POOLS
        buildPools = new Queue<Buildable>[buildables.Length];
        for (int i = 0; i < buildPools.Length; i++){
            Queue<Buildable> buildQueue = new Queue<Buildable>();
            for (int j = 0; j < buildables[i].amount; j++)
            {
                Buildable b = (Buildable)Instantiate(buildables[i].buildPrefab, transform.position, Quaternion.identity);
                buildQueue.Enqueue(b);
                b.gameObject.SetActive(false);
            }
            buildPools[i] = buildQueue;
        } 
    }

    private void Start() {
        /////PREPARE RADIAL MENU
        for (int i = 0; i < buildables.Length; i++)
        {            
            int j = i; //frescura pra C#
            radialMenu.elements[i].cost.SetText(buildables[i].buildPrefab.cost.ToString());
            radialMenu.elements[i].label = buildables[i].buildPrefab.description;
            //radialMenu.elements[i].setParentMenuLable(buildables[i].buildPrefab.description,buildables[i].buildPrefab.cost.ToString());
            //radialMenu.elements[i].button.onClick.RemoveAllListeners();
            //radialMenu.elements[i].button.onClick.AddListener(delegate{builder.HandleBuildSelection(j);});
        }
    }

    public Buildable GetBuildable(int index){
        if(buildPools == null || buildPools.Length <= index){
            Debug.Log("Invalid operation");
            return null;
        }
        Buildable b = buildPools[index].Dequeue();
        b.gameObject.SetActive(true);
        buildPools[index].Enqueue(b);
        return b;
    }
}
