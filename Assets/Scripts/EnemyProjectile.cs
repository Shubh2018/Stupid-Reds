using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    float speed = 8.0f;

    void Update()
    {
        transform.Translate(speed * Vector3.forward * Time.deltaTime);
    }
}
