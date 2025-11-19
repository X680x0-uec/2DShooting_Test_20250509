using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public GameObject firstSelectedButton;
    public RectTransform cursorImage;
    public RankingScreenController rankingScreen;
    public GameObject backgroundField;
    public GameObject blackImageForRankingScreen;
    public ManualController manualController;

    [Header("効果音")]
    public AudioClip titleCursorSound;
    public AudioClip showRankingSound;

    private GameObject currentSelectedButton;
    private bool isTitleAnimationFinished = false;

    void Start()
    {
        cursorImage.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        currentSelectedButton = null;
    }

    void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected != null && selected != currentSelectedButton && isTitleAnimationFinished)
        {
            currentSelectedButton = selected;
            UpdateCursorPosition();
        }
        else if (selected == null && currentSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    private void UpdateCursorPosition()
    {
        if (cursorImage == null || currentSelectedButton == null) return;

        SoundManager.Instance.PlaySound(titleCursorSound);

        float buttonY = currentSelectedButton.transform.position.y;

        cursorImage.transform.position = new Vector3(
            cursorImage.transform.position.x, 
            buttonY + 5f, 
            cursorImage.transform.position.z
        );
    }

    public void OnTitleAnimationEnd()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        currentSelectedButton = firstSelectedButton;
        cursorImage.gameObject.SetActive(true);
        isTitleAnimationFinished = true;
        UpdateCursorPosition();
    }

    public void OnStartButton()
    {
        if (isTitleAnimationFinished)
        {
            SceneManager.LoadScene("stage");
        }
    }

    public void OnRankingButton()
    {
        if (isTitleAnimationFinished)
        {
            if (backgroundField != null)
            {
                backgroundField.SetActive(false);
            }
            if (blackImageForRankingScreen != null)
            {
                blackImageForRankingScreen.SetActive(true);
            }
            if (rankingScreen != null)
            {
                rankingScreen.ShowRankingViewOnly();
                SoundManager.Instance.PlaySound(showRankingSound);
            }
        }
    }

    public void OnHowToPlayButton()
    {
        if (isTitleAnimationFinished)
        {
            if (backgroundField != null)
        {
            backgroundField.SetActive(false);
        }

        if (manualController != null)
        {
            manualController.gameObject.SetActive(true);
        }
        }
    }
}
