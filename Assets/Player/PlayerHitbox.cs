using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("親オブジェクトのPlayerControllerが見つかりません。");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Zako") || other.CompareTag("EnemyBullet") || other.CompareTag("Boss"))
        {
            if (playerController.IsInvincibleBySpecialSkill)
            {
                if (other.CompareTag("EnemyBullet"))
                {
                    Instantiate(playerController.debrisSpawnerPrefab, other.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                }
            }
            else
            {
                //被弾時の処理を後で追加
                if (playerController != null)
                {
                    playerController.StartBlinkEffect();
                }
                if (playerController.life <= 0)
                {
                    playerController.Gameover();
                }
                else
                {
                    playerController.life -= 1;
                    playerController.CheckAndChangeToRevengeMode();
                    InformationUIController.Instance.UpdateLivesDisplay(playerController.life);
                }
            }
        }
    }
}
