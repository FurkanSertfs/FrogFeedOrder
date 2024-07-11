using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using TMPro;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private Image blackFadeImage,
        soundOnOffImage,
        settingsGearImage,
        settingsPanel,
        soundButtonImage;

    [SerializeField]
    private Sprite soundOnSprite,
        soundOffSprite;

    [SerializeField]
    private GameObject winPanel,
        losePanel;

    [SerializeField]
    private Button nextLevelButton,
        retryButton,
        settingsButton,
        soundButton;

    [SerializeField]
    private float settingsButtonMinWidth = 120,
        settingsButtonMaxWidth = 360;

    [SerializeField]
    TextMeshProUGUI levelText;

    public Image BlackFadeImage => blackFadeImage;
    public Image SoundOnOffImage => soundOnOffImage;
    public Image SettingsGearImage => settingsGearImage;
    public RectTransform SettingsPanel => settingsPanel.rectTransform;
    public Image SoundButtonImage => soundButtonImage;
    public Sprite SoundOnSprite => soundOnSprite;
    public Sprite SoundOffSprite => soundOffSprite;
    public GameObject WinPanel => winPanel;
    public GameObject LosePanel => losePanel;
    public Button NextLevelButton => nextLevelButton;
    public Button RetryButton => retryButton;
    public Button SettingsButton => settingsButton;
    public Button SoundButton => soundButton;
    public float SettingsButtonMinWidth => settingsButtonMinWidth;
    public float SettingsButtonMaxWidth => settingsButtonMaxWidth;

    public void SetSoundIcon(bool isSoundOn)
    {
        soundOnOffImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }

    public void ShowWinPanel()
    {
        settingsPanel.gameObject.SetActive(false);
        levelText.text = "";
        blackFadeImage
            .DOFade(1, 0.5f)
            .OnComplete(() =>
            {
                winPanel.SetActive(true);
                blackFadeImage.DOFade(0, 0.25f);
            });
    }

    public void ShowLosePanel()
    {
        StartCoroutine(LevelFailedRoutine());
    }

    private IEnumerator LevelFailedRoutine()
    {
        yield return new WaitForSeconds(1);
        settingsPanel.gameObject.SetActive(false);
        levelText.text = "";
        blackFadeImage
            .DOFade(1, 0.5f)
            .OnComplete(() =>
            {
                losePanel.SetActive(true);
                blackFadeImage.DOFade(0, 0.25f);
            });
    }

    public void SetSettingsPanelActive(bool isActive)
    {
        settingsPanel.gameObject.SetActive(isActive);
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"Level {level}";
    }
}
