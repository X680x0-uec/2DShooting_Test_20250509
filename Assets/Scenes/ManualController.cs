using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManualController : MonoBehaviour
{
    [Header("説明書の設定")]
    public Image manualImageDisplay;
    public Sprite[] manualPages;

    [Header("シーン設定")]
    public string titleSceneName = "TitleScene";

    private int currentPageIndex = 0; 
    private bool isActive = false;

    [Header("効果音")]
    public AudioClip changePageSound;

    // パネルが表示された時に呼ばれる
    void OnEnable()
    {
        if (manualPages.Length == 0)
        {
            Debug.LogError("説明書の画像が設定されていません！");
            CloseAndReloadTitle();
            return;
        }

        currentPageIndex = 0;
        UpdatePageDisplay();
        isActive = true;
    }

    void Update()
    {
        if (!isActive) return;

        if (Input.GetButtonDown("Submit"))
        {
            if (currentPageIndex < manualPages.Length - 1)
            {
                currentPageIndex++;
                UpdatePageDisplay();
            }
            else
            {
                CloseAndReloadTitle();
            }
        }

        if (Input.GetButtonDown("Cancel"))
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                UpdatePageDisplay();
            }
        }
    }

    private void UpdatePageDisplay()
    {
        SoundManager.Instance.PlaySound(changePageSound);
        if (manualImageDisplay != null && manualPages.Length > 0)
        {
            manualImageDisplay.sprite = manualPages[currentPageIndex];
        }
    }

    private void CloseAndReloadTitle()
    {
        isActive = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }
}