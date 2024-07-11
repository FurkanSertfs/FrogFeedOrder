using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    CellPresenter[] cellPresenters;

    [SerializeField]
    LevelController[] levels;
    int currentLevelIndex = 0;
    int totalFrogs = 0;
    Coroutine prepareNextLevelRoutine;

    public static Action OnLevelComplete;
    public static Action OnLevelReady;
    public static Action OnNewLevelCreated;

    void OnEnable()
    {
        CellView.OnFrogSpawned += OnFrogSpawned;
        FrogTongueController.OnFrogCompleted += OnFrogCompleted;
        UIPresenter.OnPrepareNextLevel += OnPrepareNextLevel;
    }

    void OnDisable()
    {
        CellView.OnFrogSpawned -= OnFrogSpawned;
        FrogTongueController.OnFrogCompleted -= OnFrogCompleted;
        UIPresenter.OnPrepareNextLevel -= OnPrepareNextLevel;
    }

    void Start()
    {
        OnPrepareNextLevel();
    }

    void OnFrogSpawned()
    {
        totalFrogs++;
    }

    bool OnFrogCompleted()
    {
        totalFrogs--;

        if (totalFrogs <= 0)
        {
            OnLevelComplete?.Invoke();
            currentLevelIndex = (currentLevelIndex + 1) % levels.Length;
        }

        return totalFrogs <= 0;
    }

    void OnPrepareNextLevel()
    {
        if (prepareNextLevelRoutine != null)
        {
            StopCoroutine(prepareNextLevelRoutine);
        }
        totalFrogs = 0;
        prepareNextLevelRoutine = StartCoroutine(PrepareNextLevelRoutine());
    }

    IEnumerator PrepareNextLevelRoutine()
    {
        var activeLevel = levels[currentLevelIndex];

        var cellProperties = activeLevel.GetCellProperties();

        for (int i = 0; i < cellPresenters.Length; i++)
        {
            cellPresenters[i].SetCellProperties(cellProperties[i]);
        }

        activeLevel.InvokeEvents();

        OnNewLevelCreated?.Invoke();

        yield return new WaitForSeconds(1);

        OnLevelReady?.Invoke();
    }
}
