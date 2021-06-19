using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{ 
    NavMeshAgent agent;
    public Transform player;
    State currentState;
    Enemy enemy;
    public GameObject[] wayPoints;

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, agent, player, wayPoints);
        enemy = this.GetComponent<Enemy>();

        foreach(var waypoint in wayPoints)
        {
            waypoint.GetComponent<MeshRenderer>().enabled = false;
        }
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
