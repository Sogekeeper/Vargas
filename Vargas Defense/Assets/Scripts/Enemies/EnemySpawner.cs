using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject testSubject;
    public static List<GameObject> enemies;

    private void Start() {
        enemies = new List<GameObject>();
        enemies.Add(testSubject);
    }

}
