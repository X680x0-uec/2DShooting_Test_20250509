using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        //タグ"Damage"に当たった場合
        if (other.CompareTag("Damage"))
        {
            //弾消し(弾側の処理とする可能性が高いためのちに削除)
            Destroy(other.gameObject);

            //被弾時の処理を後で追加
            Debug.Log("被弾した");
        }
    }
}
