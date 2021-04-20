using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 6f;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform bulletSpawn;
    Vector3 cameraPos;

    void Start()
    {
        cameraPos = Camera.main.transform.position;
    }

    private void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hInput, 0f, vInput).normalized;
        //direction = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * direction;
        transform.Translate(direction * speed * Time.deltaTime);

        cameraPos = new Vector3(this.transform.position.x, cameraPos.y, this.transform.position.z) + new Vector3(-3f, 0f, -6f);
        Camera.main.transform.position = cameraPos;

        //bulletSpawn.position = this.transform.position + new Vector3(0f, 0f, 0.75f);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: 300f))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation);
        }
    }
}
