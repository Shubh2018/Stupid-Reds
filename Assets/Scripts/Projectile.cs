using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private LayerMask env;
    [SerializeField]
    private float maxDistance = 3.0f;

    void Update()
    {
        transform.Translate(speed * Vector3.forward * Time.deltaTime);

        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask: env))
        {
            Vector3 angleToRotate = Vector3.Reflect(ray.direction, hit.normal);
            float angleDir = 90 - Mathf.Atan2(angleToRotate.z, angleToRotate.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, angleDir, 0f);
        }

        Destroy(this.gameObject, 5.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject.GetComponent<AI>());
            Destroy(collision.gameObject.GetComponent<EnemyAttackLogic>());
            Destroy(collision.gameObject.GetComponent<NavMeshAgent>());
        }
    }
}
