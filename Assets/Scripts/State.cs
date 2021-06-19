using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State
{
    public enum STATE
    {
        IDLE, PATROL, PURSUE, ATTACK
    };

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject npc;
    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;
    protected GameObject[] wayPoints;

    protected float visDist = 3.5f;
    protected float visAngle = 45.0f;
    protected float shootDist = 7.0f;
    protected float shootAngle = 10.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Transform _player, GameObject[] _wayPoints)
    {
        wayPoints = _wayPoints;
        npc = _npc;
        agent = _agent;
        player = _player;
        stage = EVENT.ENTER;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if (stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }

        return this;
    }

    protected bool CanSeePlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (angle < visAngle)
            return true;
        else
            return false;
    }

    protected bool CanShootPlayer()
    {
        Vector3 direction = player.position - npc.transform.position;
        float angle = Vector3.Angle(direction, npc.transform.forward);

        if (angle < shootAngle)
            return true;
        else
            return false;
    }

    protected void FacePlayer()
    {
        npc.transform.LookAt(player.position);
    }
}

public class Idle : State
{
    public Idle(GameObject _npc, NavMeshAgent _agent, Transform _player, GameObject[] _wayPoints) : base(_npc, _agent, _player, _wayPoints)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(npc, agent, player, wayPoints);
            stage = EVENT.EXIT;
        }
        //base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Patrol : State
{
    int currentIndex = -1;

    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player, GameObject[] _wayPoints) : base(_npc, _agent, _player, _wayPoints)
    {
        name = STATE.PATROL;
        agent.speed = 4;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if (agent.remainingDistance < 1)
        {
            currentIndex = Random.Range(0, wayPoints.Length);
            agent.SetDestination(wayPoints[currentIndex].transform.position);
        }

        if(Vector3.Distance(npc.transform.position, player.position) <= visDist && CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player, wayPoints);
            stage = EVENT.EXIT;
        }
        //base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Pursue : State
{
    public Pursue(GameObject _npc, NavMeshAgent _agent, Transform _player, GameObject[] _wayPoints) : base(_npc, _agent, _player, _wayPoints)
    {
        name = STATE.PURSUE;
        agent.speed = 5;
        agent.isStopped = false;
        agent.stoppingDistance = 2.0f;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);

        if(Vector3.Distance(npc.transform.position, player.position) > visDist)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Length)].transform.position);
            nextState = new Patrol(npc, agent, player, wayPoints);
            stage = EVENT.EXIT;
        }

        if (Vector3.Distance(npc.transform.position, player.position) <= shootDist && CanShootPlayer())
        {
            nextState = new Attack(npc, agent, player, wayPoints);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Attack : State
{
    public Attack(GameObject _npc, NavMeshAgent _agent, Transform _player, GameObject[] _wayPoints) : base(_npc, _agent, _player, _wayPoints)
    {
        name = STATE.ATTACK;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        FacePlayer();
        
        if (Vector3.Distance(npc.transform.position, player.position) > shootDist)
        {
            nextState = new Pursue(npc, agent, player, wayPoints);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}