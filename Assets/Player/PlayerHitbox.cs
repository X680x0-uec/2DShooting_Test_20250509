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
        if (other.CompareTag("Zako") || other.CompareTag("EnemyBullet"))
        {
            //被弾時の処理を後で追加
            if (playerController != null)
            {
                playerController.StartBlinkEffect();
            }
            playerController.life -= 1;
            Debug.Log("life:" + playerController.life);
        }
    }
}
