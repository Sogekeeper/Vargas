using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public Enemy enemyStats;
    public Animator anim;

    public State remainState;

    public NavMeshAgent agent{get; private set;}
    public Transform trans{get; private set;}

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        trans = GetComponent<Transform>();
    }

    private void Start() {
        agent.updatePosition = false;
    }

    private void Update() {
        currentState.UpdateState(this);

        agent.nextPosition = transform.position;
        transform.rotation = agent.transform.rotation;

        anim.SetFloat("speed", agent.desiredVelocity.magnitude);
    }

    public void TransitionState(State nextState){
        if(nextState != remainState){
            currentState = nextState;
        }
    }
}
