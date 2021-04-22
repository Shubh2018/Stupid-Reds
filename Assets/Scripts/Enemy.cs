using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private float fireRate = 1.5f;
    private float nextTimeToFire = 0f;
    public int id;

    private void Start()
    {
        GameEnvironment.instance.onShoot += Shoot;
    }

    public void Shoot(int id)
    {
        if(id == this.id)
        {
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation);
            }
        }  
    }

    private void OnDestroy()
    {
        GameEnvironment.instance.onShoot -= Shoot;
    }
}
