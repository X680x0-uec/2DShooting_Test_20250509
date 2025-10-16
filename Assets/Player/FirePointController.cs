using UnityEngine;

public class FirePointController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public float fireRateMultiplier = 1.0f;

    private float nextFireTime = 0f;
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void SetFireRateMultiplier(float rate)
    {
        fireRateMultiplier = rate;
    }

    public void Fire()
    {
        if (bulletPrefab != null && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate * fireRateMultiplier;

            GameObject newBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            PlayerBulletController bulletScript = newBullet.GetComponent<PlayerBulletController>();
            if (bulletScript != null && playerController != null)
            {
                bulletScript.Initialize(playerController.attackMultiplier);
            }
        }
    }
}