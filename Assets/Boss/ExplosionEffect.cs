using UnityEngine;
using System.Collections;

public class ExplosionEffect : MonoBehaviour
{
    [Header("スプライト設定")]
    [SerializeField] private Sprite[] explosionSprites; // part1〜8を入れる
    [SerializeField] private float frameInterval = 0.1f; // コマ間の時間
    [SerializeField] private bool destroyAfterPlay = true; // 終了後削除

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(PlayExplosion());
    }

    private IEnumerator PlayExplosion()
    {
        if (explosionSprites == null || explosionSprites.Length == 0)
        {
            Debug.LogWarning("[ExplosionEffect] スプライトが設定されていません。");
            yield break;
        }

        foreach (var sprite in explosionSprites)
        {
            spriteRenderer.sprite = sprite;
            yield return new WaitForSeconds(frameInterval);
        }

        if (destroyAfterPlay)
            Destroy(gameObject);
    }
}
