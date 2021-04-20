using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : MonoBehaviour
{
    public static GameEnvironment instance;
    public List<GameObject> wayPoints = new List<GameObject>();

    private void Awake()
    {
        instance = this;
        wayPoints.AddRange(GameObject.FindGameObjectsWithTag("Waypoint"));
        //wayPoints = wayPoints.OrderBy(wayPoints => wayPoints.name).ToList();
        
        foreach(var waypoint in wayPoints)
        {
            waypoint.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
