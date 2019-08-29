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

    public static BuildingsDatabase Instance;     

    private void Awake() {
        if(Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        if(buildables == null || buildables.Length <= 0) return;

        buildPools = new Queue<Buildable>[buildPools.Length];
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
