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
        Destroy(this.gameObject, 5.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            Destroy(collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
