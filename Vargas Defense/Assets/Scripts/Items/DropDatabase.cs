using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDatabase : MonoBehaviour
{
    
    [Header("Resources")]    
    public int numberOfInstances;
    public Resource[] prefabsValue1;
    public Resource[] prefabsValue5;
    public Resource[] prefabsValue10;
    
    private Queue<Resource> poolValue1;
    private Queue<Resource> poolValue5;
    private Queue<Resource> poolValue10;

    public static DropDatabase Instance; 

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        poolValue1 = new Queue<Resource>();
        poolValue5 = new Queue<Resource>();
        poolValue10 = new Queue<Resource>();

        InitPool(prefabsValue1, poolValue1,1);
        InitPool(prefabsValue5, poolValue5,5);
        InitPool(prefabsValue10, poolValue10,10);
        
    }

    public void DropAmount(int targetAmount, Vector3 location){
        int remainingAmount = targetAmount;
        float forceToDrop = 2f;
        while(remainingAmount >= 10){
            Resource r = GetFromTargetPool(poolValue10);
            r.KickOff(forceToDrop,location);
            remainingAmount -= 10;
        }
        while(remainingAmount >= 5){
            Resource r = GetFromTargetPool(poolValue5);
            r.KickOff(forceToDrop,location);
            remainingAmount -= 5;
        }
        while(remainingAmount >= 1){
            Resource r = GetFromTargetPool(poolValue1);
            r.KickOff(forceToDrop,location);
            remainingAmount -= 1;
        }
    }

    private Resource GetFromTargetPool(Queue<Resource> pool){
        Resource r = pool.Dequeue();
        r.gameObject.SetActive(true);
        pool.Enqueue(r);
        return r;
    }

    private void InitPool(Resource[] resourceTypes, Queue<Resource> targetPool, int targetValue){
        if(resourceTypes != null && resourceTypes.Length > 0){
            for (int i = 0; i < numberOfInstances; i++)
            {
                Resource r = (Resource)Instantiate(resourceTypes[Random.Range(0,resourceTypes.Length)], transform.position, Quaternion.identity);                
                r.SetUp(targetValue);
                targetPool.Enqueue(r);
                r.gameObject.SetActive(false);
            }
        }
    }
    

}
