using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Action<PaperController> OnPaperSpawned { get; set; }
    public Action<FanController> OnFanUpdated { get; set; }
    public Action<ScoreManager> OnScoreUpdated { get; set; }
    public Action OnGameStart { get; set; }
    public Action OnPaperScored { get; set; }
    public Action OnPaperFailScore { get; set; }

    [Header("Game Managers")]

    [SerializeField]
    private FanManager fanManager;

    [SerializeField]
    private PaperManager paperManager;

    [SerializeField]
    private ScoreManager scoreManager;

    [SerializeField]
    private MainGameUI mainGameUI;

    [SerializeField]
    private CameraManager cameraManager;

    [Header("Game Settings")]

    [SerializeField]
    private GameSettingsData gameSettingsData;

    protected override void Awake()
    {
        base.Awake();

        Physics.gravity = gameSettingsData.Gravity;
        paperManager.OnPaperSpawned += PaperSpawned;
        paperManager.OnPaperScored += PaperScored;
        paperManager.OnPaperFailScore += PaperFailScore;
        fanManager.OnFanUpdated += FanUpdated;
        scoreManager.OnScoreUpdated += ScoreUpdated;
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void StartGame()
    {
        cameraManager.TransitionGameCamera();
        OnGameStart?.Invoke();
    }

    public void ExitGame() => Application.Quit();
    private void PaperSpawned(PaperController paperController) => OnPaperSpawned?.Invoke(paperController);
    private void PaperScored() => OnPaperScored?.Invoke();
    private void PaperFailScore() => OnPaperFailScore?.Invoke();
    private void FanUpdated(FanController fanController) => OnFanUpdated?.Invoke(fanController);
    private void ScoreUpdated(ScoreManager scoreManager) => OnScoreUpdated?.Invoke(scoreManager);

    private void Dispose()
    {
        paperManager.OnPaperSpawned -= PaperSpawned;
        paperManager.OnPaperScored -= PaperScored;
        paperManager.OnPaperFailScore -= PaperFailScore;
        fanManager.OnFanUpdated -= FanUpdated;
        scoreManager.OnScoreUpdated -= ScoreUpdated;
    }
}
