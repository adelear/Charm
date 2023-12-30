using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject characterProfile;
    [SerializeField] GameObject introDialogue;
    [SerializeField] GameObject outroDialogue;
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioManager manager;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;

    private bool canShoot = true;

    private void Update()
    {
        if (characterProfile != null)
        {
            if (Input.GetButtonDown("Fire1") && canShoot && !characterProfile.activeSelf)
            {
                if (!introDialogue.activeSelf)
                {
                    if (!outroDialogue.activeSelf) Fire();
                }
            }
        }
    }

    private void Fire()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            manager.PlayOneShot(shootSound, false); 
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
}
