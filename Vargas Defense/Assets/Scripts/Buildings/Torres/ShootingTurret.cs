using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTurret : MonoBehaviour, IBuildBehaviour
{
    [Header("Stats")]
    public float range = 5f;
    public int damage = 1;
    public float fireRate = 1f;
    public float dangerZone = 1f;
    public float turningSpeed = 0.8f;
    public float maxAngleToAttack = 10f;

    [Header("Parts")]
    public GameObject bulletPrefab;
    public Transform movingPartY;
    public Transform movingPartX;
    public Transform bulletSpawner;
    
    private bool ready = false;
    
    private Transform trans;
    private GameObject[] bulletPool;
    private int currentBulletIndex = 0;
    private float attackTimer = 0;

    private Enemy target;

    private void Start() {
        trans = GetComponent<Transform>();
        bulletPool = new GameObject[4];
        currentBulletIndex = 0;
        for (int i = 0; i < bulletPool.Length; i++)
        {
          bulletPool[i] = Instantiate(bulletPrefab, trans.position, Quaternion.identity);  
          bulletPool[i].SetActive(false);
        }
    }

    public void StartBuilding()
    {
        ready = false;
        StopAllCoroutines();        
    }
    public void FinishedBuilding()
    {
        ready = true;
        StartCoroutine(CheckForEnemies());
    }

    private void Update() {
        if(ready){
            TrackTarget();
            TryToAttack();
        }
    }

    private void TrackTarget(){
        if(!target || !target.gameObject.activeInHierarchy) return;
        Vector3 targetDir = (target.gameObject.transform.position - movingPartX.position) + new Vector3(0, target.offsetY, 0);
        Quaternion toRot = Quaternion.LookRotation(targetDir, Vector3.up);
        //toRot = Quaternion.Euler(0,toRot.eulerAngles.y,0);
        movingPartX.rotation = Quaternion.Slerp(movingPartX.rotation, Quaternion.Euler(toRot.eulerAngles.x,movingPartY.rotation.eulerAngles.y,movingPartY.rotation.eulerAngles.z), turningSpeed );
        movingPartY.rotation = Quaternion.Slerp(movingPartY.rotation, Quaternion.Euler(movingPartY.rotation.eulerAngles.x,toRot.eulerAngles.y,movingPartY.rotation.eulerAngles.z), turningSpeed );
    }

    private void TryToAttack(){
        if(!target || !target.gameObject.activeInHierarchy) return;
        if(attackTimer > 0){
            attackTimer -= Time.deltaTime;
            return;
        }
        Vector3 dirTarget = (target.gameObject.transform.position + new Vector3(0,target.offsetY,0)) - movingPartX.position;
        float angleToTarget = Vector3.Angle(movingPartY.forward, dirTarget);
        if(angleToTarget <= maxAngleToAttack){
            Debug.Log("fire");
            bulletPool[currentBulletIndex].SetActive(true);
            bulletPool[currentBulletIndex].transform.position = bulletSpawner.position;
            bulletPool[currentBulletIndex].transform.rotation = bulletSpawner.rotation;
            bulletPool[currentBulletIndex].GetComponent<Bullet>().InitBullet(damage);
        }
        attackTimer = fireRate;
    }

    private IEnumerator CheckForEnemies(){
        while(true){
            GameObject g = GetClosestEnemy();
            if(g) target = g.GetComponent<Enemy>();
            else target = null;
            yield return new WaitForSeconds(1f);
        }
    }

    public GameObject GetClosestEnemy(){        
        float shortestDist = Mathf.Infinity;
        float shortestAngle = Mathf.Infinity;
        GameObject potentialTarget = null;
        if(EnemySpawner.enemies != null && EnemySpawner.enemies.Count > 0){
            for(int i = 0; i < EnemySpawner.enemies.Count; i++){
                float curDist = Vector3.Distance(trans.position,EnemySpawner.enemies[i].transform.position);
                Vector3 dir = EnemySpawner.enemies[i].transform.position - trans.position;
                float curAngle = Vector3.Angle(movingPartY.forward,dir);
                if(curDist <= range && EnemySpawner.enemies[i].activeInHierarchy){
                    if(curAngle < shortestAngle){
                        curAngle = shortestAngle;
                        if(curDist < shortestDist || curDist < dangerZone){
                            curDist = shortestDist;
                            potentialTarget = EnemySpawner.enemies[i].gameObject;                             
                        }
                    }
                }
            }
        }
        //if(potentialTarget) print("Found: "+potentialTarget.name);
        //else print("Nothing Found");
        return potentialTarget;

    }

}
