using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIPresenter
{
    private readonly UIView view;
    private readonly UIModel model;
    private Coroutine settingsButtonRoutine;

    public static Action OnPrepareNextLevel;

    public UIPresenter(UIView view, UIModel model)
    {
        this.view = view;
        this.model = model;

        GameManager.OnLevelComplete += LevelComplete;
        GameManager.OnLevelReady += LevelPrepared;
        MovePresenter.OnNoMovesLeft += LevelFailed;
        LevelController.OnLevelStart += SetLevelText;

        InitializeButtons();
    }

    private void SetLevelText(int level)
    {
        view.SetLevelText(level);
    }

    private void InitializeButtons()
    {
        view.NextLevelButton.onClick.AddListener(PrepareNextLevel);
        view.RetryButton.onClick.AddListener(PrepareNextLevel);
        view.SettingsButton.onClick.AddListener(OpenSettings);
        view.SoundButton.onClick.AddListener(SoundOnOff);
    }

    private void OpenSettings()
    {
        if (settingsButtonRoutine != null)
        {
            view.StopCoroutine(settingsButtonRoutine);
            DOTween.Kill(view.SettingsPanel);
            DOTween.Kill(view.SoundOnOffImage);
            DOTween.Kill(view.SettingsGearImage);
        }

        settingsButtonRoutine = view.StartCoroutine(SettingsButtonRoutine());
    }

    private void SoundOnOff()
    {
        SoundManager.Instance.ToggleSound();
        model.IsSoundOn = !model.IsSoundOn;
        view.SetSoundIcon(model.IsSoundOn);
    }

    private IEnumerator SettingsButtonRoutine()
    {
        model.IsSettingsOn = !model.IsSettingsOn;
        float duration = 0.5f;
        float timer = 0;
        float startWidth = view.SettingsPanel.sizeDelta.x;
        float targetWidth = model.IsSettingsOn ? view.SettingsButtonMaxWidth : view.SettingsButtonMinWidth;
        float height = view.SettingsPanel.sizeDelta.y;

        if (model.IsSettingsOn)
        {
            view.SoundOnOffImage
                .DOFade(1, duration * 2)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    view.SoundButtonImage.raycastTarget = true;
                });
            view.SoundOnOffImage.rectTransform.DOShakeScale(0.5f, 0.5f, 10, 90);
        }
        else
        {
            view.SoundOnOffImage.DOFade(0, duration / 2).SetEase(Ease.OutCubic);
            view.SoundButtonImage.raycastTarget = false;
        }
        view.SettingsGearImage.rectTransform.DORotate(new Vector3(0, 0, model.IsSettingsOn ? -180 : 0), duration);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float width = Mathf.Lerp(startWidth, targetWidth, t);
            view.SettingsPanel.sizeDelta = new Vector2(width, height);
            yield return null;
        }
    }

    private void LevelComplete()
    {
        view.ShowWinPanel();
    }

    private void LevelFailed()
    {
        view.ShowLosePanel();
    }

    private void PrepareNextLevel()
    {
        view.BlackFadeImage
            .DOFade(1, 0.5f)
            .OnComplete(() =>
            {
                view.WinPanel.SetActive(false);
                view.LosePanel.SetActive(false);
                view.SetSettingsPanelActive(true);

                OnPrepareNextLevel?.Invoke();
            });
    }

    private void LevelPrepared()
    {
        view.BlackFadeImage.DOFade(0, 0.5f);
    }

    public void Cleanup()
    {
        GameManager.OnLevelComplete -= LevelComplete;
        GameManager.OnLevelReady -= LevelPrepared;
        MovePresenter.OnNoMovesLeft -= LevelFailed;
        LevelController.OnLevelStart -= SetLevelText;
    }
}
