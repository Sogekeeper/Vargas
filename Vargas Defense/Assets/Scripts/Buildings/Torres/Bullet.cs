using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public BoxCollider col;
    public ParticleSystem impactPart;
    
    private int dmg;

    private Rigidbody rb;
    private Transform trans;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
    }

    public void InitBullet(int targetDamage, float bSpeed = 10f){
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        dmg = targetDamage;    
        col.gameObject.SetActive(true);
        if(impactPart)impactPart.gameObject.SetActive(false);

        rb.velocity = trans.forward * bSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        Enemy e = other.GetComponent<Enemy>();
        if(e) e.TakeDamage(dmg);
        col.gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        if(impactPart){
            impactPart.gameObject.SetActive(true);
            impactPart.Stop();
            impactPart.Play();
        }

    }
}
