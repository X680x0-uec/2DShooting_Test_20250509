using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class NameInputController : MonoBehaviour
{
    public TextMeshProUGUI currentNameText;
    public int maxNameLength = 10;
    public GameObject firstSelectedButton;
    public RankingScreenController rankingScreen;
    public RectTransform cursorFrame;

    private StringBuilder nameBuilder = new StringBuilder();
    private GameObject lastSelected;

    [Header("効果音")]
    public AudioClip cursorMoveSound;
    public AudioClip clickSound;
    public AudioClip submitSound;
    public AudioClip cancelSound;

    private AudioSource sfxSource;

    void Awake()
    {
        sfxSource = GetComponent<AudioSource>();

        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
    }

    void Update()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && currentSelected != lastSelected)
        {
            UpdateCursorPosition(currentSelected);
            lastSelected = currentSelected;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            PressBackspace();
        }
    }

    void OnEnable()
    {
        if (cursorFrame != null)
        {
            cursorFrame.gameObject.SetActive(true);
        }

        nameBuilder.Clear();
        UpdateNameText();

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);

            lastSelected = null;
        }
    }

    void OnDisable()
    {
        if (cursorFrame != null)
        {
            cursorFrame.gameObject.SetActive(false);
        }
    }

    public void PressCharacter(string character)
    {
        if (nameBuilder.Length < maxNameLength)
        {
            nameBuilder.Append(character);
            UpdateNameText();
        }
        SoundManager.Instance.PlaySound(clickSound);
    }

    public void PressBackspace()
    {
        if (nameBuilder.Length > 0)
        {
            nameBuilder.Length--;
            UpdateNameText();
        }
        SoundManager.Instance.PlaySound(cancelSound);
    }

    public void PressEnd()
    {
        SoundManager.Instance.PlaySound(submitSound);
        rankingScreen.RegisterScore(nameBuilder.ToString());
    }

    private void UpdateNameText()
    {
        string current = nameBuilder.ToString();
        string padded = current.PadRight(maxNameLength, '_');
        currentNameText.text = padded;
    }

    private void UpdateCursorPosition(GameObject targetButton)
    {
        if (cursorFrame == null || targetButton == null)
        {
            return;
        }

        cursorFrame.position = targetButton.transform.position;

        RectTransform buttonRect = targetButton.GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            cursorFrame.sizeDelta = buttonRect.sizeDelta;
        }
        SoundManager.Instance.PlaySound(cursorMoveSound);
    }
}
