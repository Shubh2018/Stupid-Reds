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

    protected float visDist = 10.0f;
    protected float visAngle = 30.0f;
    protected float shootDist = 7.0f;
    protected float shootAngle = 10.0f;

    public State(GameObject _npc, NavMeshAgent _agent, Transform _player)
    {
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
    public Idle(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
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
            nextState = new Patrol(npc, agent, player);
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

    public Patrol(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        currentIndex = 0;
        base.Enter();
    }

    public override void Update()
    {
        if(agent.remainingDistance < 1)
        {
            if (currentIndex >= GameEnvironment.instance.wayPoints.Count - 1)
                currentIndex = 0;
            else
            {
                currentIndex = Random.Range(0, GameEnvironment.instance.wayPoints.Count);
                currentIndex++;
            }

            agent.SetDestination(GameEnvironment.instance.wayPoints[currentIndex].transform.position);
        }

        if(Vector3.Distance(npc.transform.position, player.position) <= visDist && CanSeePlayer())
        {
            nextState = new Pursue(npc, agent, player);
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
    public Pursue(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
    {
        name = STATE.PURSUE;
        agent.speed = 4;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        agent.stoppingDistance = 4.0f;

        if(Vector3.Distance(npc.transform.position, player.position) > visDist)
        {
            nextState = new Patrol(npc, agent, player);
            stage = EVENT.EXIT;
        }

        if (Vector3.Distance(npc.transform.position, player.position) <= shootDist && CanShootPlayer())
        {
            nextState = new Shoot(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Shoot : State
{
    public Shoot(GameObject _npc, NavMeshAgent _agent, Transform _player) : base(_npc, _agent, _player)
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
            nextState = new Pursue(npc, agent, player);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}