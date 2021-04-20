using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    State currentState;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, agent, player);
    }

    void Update()
    {
        currentState = currentState.Process();
    }
}
