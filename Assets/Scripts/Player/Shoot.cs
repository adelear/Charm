using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;

    private bool canShoot = true;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
