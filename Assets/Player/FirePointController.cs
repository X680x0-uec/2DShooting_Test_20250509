using UnityEngine;

//継承を想定して設計済み
public class FirePointController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public float fireRateMultiplier = 1.0f;

    //拡散弾用設定(他オプションは初期値のままで問題なし)
    public int wayCount = 1;
    public float spreadAngle = 30f;

    protected float nextFireTime = 0f;
    protected PlayerController playerController;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
    }

    public void SetFireRateMultiplier(float rate)
    {
        fireRateMultiplier = rate;
    }

    public void SetSpreadSetting(int way, float angle)
    {
        wayCount = way;
        spreadAngle = angle;
    }

    public void Fire()
    {
        if (bulletPrefab != null && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate * fireRateMultiplier;

            float startAngle = (wayCount > 1) ? -spreadAngle / 2 : 0;
            float angleStep = (wayCount > 1) ? spreadAngle / (wayCount - 1) : 0;


            for (int i = 0; i < wayCount; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Quaternion bulletRotation = transform.rotation * Quaternion.Euler(0, 0, currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, transform.position, bulletRotation);

                PlayerBulletController bulletScript = newBullet.GetComponent<PlayerBulletController>();
                if (bulletScript != null && playerController != null)
                {
                    bulletScript.Initialize(playerController.GetAttackMultiplier());
                }
            }
        }
    }
}