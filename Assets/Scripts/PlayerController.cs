using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private float maxDistance = 10.0f;
    [SerializeField]
    private LayerMask env;
    public bool canShoot = true;
    public int bulletCount = 10;
    private bool isDead;
    private Transform cameraPos;
    private Vector3 offset;
    public int ammoCount = 20;
    public int maxBulletCount = 10;
    public float timeToReload = 5.0f;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    private void Start()
    {
        cameraPos = Camera.main.transform;
        offset = cameraPos.position - transform.position;
    }

    private void Update()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(hInput, 0f, vInput).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            Vector3 target = hit.point;
            target.y = transform.position.y;
            transform.LookAt(target);

            if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
            {
                Instantiate(projectile, bulletSpawn.position, bulletSpawn.rotation);
                bulletCount--;
            }
        }

        if (bulletCount < 1 && ammoCount < 1)
            canShoot = false;

        if ((Input.GetKeyDown(KeyCode.R) && bulletCount < maxBulletCount && ammoCount > 0) || (bulletCount < 1 && ammoCount > 0))
        {
            canShoot = false;
            StartCoroutine(Reload());
        }
    }

    private void LateUpdate()
    {
        cameraPos.position = transform.position + offset;
    }

    private void OnDestroy()
    {
        isDead = true;
        UIManager.instance.LevelClear = false;
    }

    private IEnumerator Reload()
    {
        Debug.Log("Reload");

        if(ammoCount < maxBulletCount)
        {
            ammoCount -= maxBulletCount - bulletCount;
            bulletCount += maxBulletCount - bulletCount;

            if (ammoCount < 0)
                ammoCount = 0;
        }

        else
        {
            ammoCount = bulletCount + ammoCount;
            bulletCount = maxBulletCount;
            ammoCount -= maxBulletCount;
        }

        UIManager.instance.EnableReloadText();

        yield return new WaitForSeconds(timeToReload);

        Debug.Log("5 seconds passed");

        canShoot = true;

        UIManager.instance.EnableReloadText();
    }
}
