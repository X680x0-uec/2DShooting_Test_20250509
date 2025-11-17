using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BossHP))]
public class BossEnterMovement : MonoBehaviour
{
    [Header("入場設定")]
    [Tooltip("右側からどれだけ外側(画面幅比率)から開始するか。1 = 画面幅分右")]
    [SerializeField] private float enterOffsetViewportX = 0.2f;

    [Tooltip("入場にかける時間（秒） — 小さいほど速い")]
    [SerializeField] private float enterDuration = 1.2f;

    [Tooltip("到着後に少しだけ待つ（演出の余韻）")]
    [SerializeField] private float waitAfterArrive = 0.15f;

    [Header("補間モード")]
    [SerializeField] private bool useCurve = false;
    [SerializeField] private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("SmoothDamp モード設定（useCurve = false のとき）")]
    [SerializeField] private float smoothDampTime = 0.25f;

    [Header("参照")]
    [SerializeField] private BossHP bossHP;

    public System.Action OnEnterFinished;

    private Vector3 targetPos;
    private Vector3 startPos;
    private Vector3 velocity = Vector3.zero;

    private void Reset()
    {
        moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }

    private void Start()
    {
        if (bossHP == null) bossHP = GetComponent<BossHP>();

        // 最終位置
        targetPos = transform.position;

        // カメラから右外の開始位置を計算
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(
                1f, 0.5f,
                Mathf.Abs(cam.transform.position.z - transform.position.z)
            ));

            float screenWidthWorld = Mathf.Abs(
                rightEdge.x -
                cam.ViewportToWorldPoint(new Vector3(0f, 0.5f,
                Mathf.Abs(cam.transform.position.z - transform.position.z))).x
            );

            startPos = new Vector3(
                rightEdge.x + enterOffsetViewportX * screenWidthWorld,
                targetPos.y,
                targetPos.z
            );
        }
        else
        {
            startPos = targetPos + new Vector3(Mathf.Abs(enterOffsetViewportX), 0f, 0f);
        }

        transform.position = startPos;

        // 無敵化
        bossHP.SetInvincible(true);

        StartCoroutine(EnterRoutine());
    }

    private IEnumerator EnterRoutine()
    {
        float t = 0f;

        if (useCurve)
        {
            while (t < 1f)
            {
                float eval = moveCurve.Evaluate(t);
                transform.position = Vector3.Lerp(startPos, targetPos, eval);
                t += Time.deltaTime / Mathf.Max(0.0001f, enterDuration);
                yield return null;
            }
        }
        else
        {
            float elapsed = 0f;
            while (elapsed < enterDuration)
            {
                transform.position = Vector3.SmoothDamp(
                    transform.position,
                    targetPos,
                    ref velocity,
                    smoothDampTime,
                    Mathf.Infinity,
                    Time.deltaTime
                );
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        transform.position = targetPos;
        yield return new WaitForSeconds(waitAfterArrive);

        OnEnterFinished?.Invoke();
    }

    public void CancelEnter()
    {
        StopAllCoroutines();
    }
}
