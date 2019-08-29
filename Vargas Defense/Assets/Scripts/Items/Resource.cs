using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Resource : MonoBehaviour
{
    private int value;
    private Rigidbody rb;

    private void Start() {
        
    }

    public void SetUp(int targetValue){
        rb = GetComponent<Rigidbody>();
        value = targetValue;
    }

    public void KickOff(float forceAmount){
        rb.AddForce(Random.insideUnitSphere * forceAmount, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other) {
        FPSBuilderManager builder = other.GetComponent<FPSBuilderManager>();
        if(builder){
            builder.currentResources += value;
            gameObject.SetActive(false);
        }
    }
}
