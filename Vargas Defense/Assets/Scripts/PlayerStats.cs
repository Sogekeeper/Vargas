using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int totalLife = 20;
    public int currentLife{get; private set;}

    [Header("Weapon Settings")]
    public int damage = 5;
    public float weaponCooldown;
    public float weaponTimer {get; private set;}
    public Transform bulletSpawner;
    public PlayerBullet bulletObject;
    

    private void Start() {
        currentLife = totalLife;
        weaponTimer = weaponCooldown;
        
    }

    private void Update() {
        if(weaponTimer < weaponCooldown){
            weaponTimer += Time.deltaTime;
        }    
    }

    public void TakeDamage(int dmg){
        currentLife -= dmg;
        if(currentLife <= 0){
            //Die()
            gameObject.SetActive(false);
        }
    }

    public void Fire(){
        if(weaponTimer < weaponCooldown) return;
        weaponTimer = 0;
        bulletObject.transform.position = bulletSpawner.position;
        bulletObject.transform.rotation = bulletSpawner.rotation;
        bulletObject.InitBullet(damage,20);
    }

}
