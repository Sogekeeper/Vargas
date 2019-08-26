using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour
{
    public State currentState;
    public Enemy enemyStats;
    public NavMeshAgent agent;
    public Animator anim;

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
}
