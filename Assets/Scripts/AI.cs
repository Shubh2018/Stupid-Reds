using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{ 
    public int id;
    NavMeshAgent agent;
    public Transform player;
    State currentState;
    Enemy enemy;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, agent, player);
        enemy = this.GetComponent<Enemy>();
    }

    void Update()
    {
        currentState = currentState.Process();

        if(currentState.name == State.STATE.ATTACK)
        {
            GameEnvironment.instance.OnShoot(enemy.id);
        }
    }
}
