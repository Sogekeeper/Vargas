using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] testSubjects;
    public static List<GameObject> enemies;

    private void Start() {
        enemies = new List<GameObject>();
        //enemies.Add(testSubject);
        for (int i = 0; i < testSubjects.Length; i++)
        {
            enemies.Add(testSubjects[i]);
        }
    }

}
