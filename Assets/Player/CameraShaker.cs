using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker Instance { get; private set; }

    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    //Startより先に呼び出し
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            originalPosition = transform.localPosition;
        }
        else
        {
            Destroy(gameObject); //既にインスタンスが生成済みなら自身を削除
        }
    }

    public void ShakeCamera(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; //1フレーム待機
        }

        transform.localPosition = originalPosition;
    }
}
