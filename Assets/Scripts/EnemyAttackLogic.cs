using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private float fireRate = 1.5f;
    private float nextTimeToFire = 0f;

    public void Shoot()
    {
        if(Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}
