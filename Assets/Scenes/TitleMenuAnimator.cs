using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleMenuAnimator : MonoBehaviour
{
    public TitleManager titleManager;

    [Header("アニメーション対象UI")]
    public RectTransform uiToAnimate;
    public float startOffsetY = 500f;
    public float animationDuration = 1.0f;

    [Header("背景フィールドの初期状態")]
    public Image backgroundFieldImage;
    public float targetAlpha = 0.6f;

    private Vector2 targetPosition;

    void Awake()
    {
        targetPosition = uiToAnimate.anchoredPosition;

        uiToAnimate.anchoredPosition = targetPosition + new Vector2(0, startOffsetY);

        if (backgroundFieldImage != null)
        {
            Color currentColor = backgroundFieldImage.color;
            backgroundFieldImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
        }
    }

    void Start()
    {
        // アニメーションを開始
        StartCoroutine(AnimateUI());
    }

    private IEnumerator AnimateUI()
    {
        float timer = 0f;
        
        Vector2 startPosition = uiToAnimate.anchoredPosition;
        float currentAlpha = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / animationDuration;

            float easedProgress = 1f - (1f - progress) * (1f - progress);
            
            uiToAnimate.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, easedProgress);

            if (backgroundFieldImage != null)
            {
                currentAlpha = Mathf.Lerp(0f, targetAlpha, progress);
                Color currentColor = backgroundFieldImage.color;
                backgroundFieldImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentAlpha);
            }

            yield return null;
        }

        uiToAnimate.anchoredPosition = targetPosition;
        titleManager.OnTitleAnimationEnd();
        if (backgroundFieldImage != null)
        {
            Color currentColor = backgroundFieldImage.color;
            backgroundFieldImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        }
    }
}