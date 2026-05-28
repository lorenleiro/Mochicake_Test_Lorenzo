using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayUI : BaseUI
{
    private VisualElement scoreBar;
    private VisualElement settingsContainer;
    private VisualElement skinVisual;
    private Label scoreText;
    private Label bestScoreText;
    private Label totalPointsText;
    private Label multiplierText;
    private Button settingsButton;
    private Button previousLevelButton;
    private Button nextLevelButton;
    private Button changeSkinButton;
    private Button closeSettingsButton;
    private Label levelText;
    private GameSettingsData gameSettingsData;
    private int currentLevel;

    public GameplayUI(VisualElement root, GameSettingsData gameSettingsData) : base()
    {
        this.gameSettingsData = gameSettingsData;

        scoreBar = root.Q<VisualElement>("score_bar");
        settingsContainer = root.Q<VisualElement>("settings_container");
        skinVisual = root.Q<VisualElement>("skin_visual");
        scoreText = root.Q<Label>("score_text");
        bestScoreText = root.Q<Label>("best_score_text");
        levelText= root.Q<Label>("level_text");
        totalPointsText = root.Q<Label>("total_points_text");
        multiplierText = root.Q<Label>("multiplier_text");
        settingsButton = root.Q<Button>("settings_button");
        changeSkinButton = root.Q<Button>("change_skin_button");
        previousLevelButton = root.Q<Button>("previous_level_button");
        nextLevelButton = root.Q<Button>("next_level_button");
        closeSettingsButton = root.Q<Button>("close_settings");

        settingsButton.clicked += ShowSettings;
        closeSettingsButton.clicked += HideSettings;
        nextLevelButton.clicked += NextLevel;
        previousLevelButton.clicked += PreviousLevel;

        GameManager.Instance.OnScoreUpdated += UpdateScore;
    }

    public override void Dispose()
    {
        GameManager.Instance.OnScoreUpdated -= UpdateScore;
        settingsButton.clicked -= ShowSettings;
        closeSettingsButton.clicked -= HideSettings;
        nextLevelButton.clicked -= NextLevel;
        previousLevelButton.clicked -= PreviousLevel;
    }

    private void UpdateScore(ScoreManager scoreManager)
    {
        scoreText.text = scoreManager.Score.ToString();
        bestScoreText.text = scoreManager.BestScore.ToString();
        totalPointsText.text = scoreManager.TotalPoints.ToString();
        multiplierText.text = scoreManager.Multiplier.ToString();

        if (scoreManager.Score > 0)
        {
            scoreBar.style.width = Length.Percent((float)scoreManager.RewardPoints / (float)scoreManager.RewardMaxPoints * 100);
        }
        else
        {
            scoreBar.style.width = Length.Percent(0);
        }
    }

    private void ShowSettings()
    {
        settingsContainer.RemoveFromClassList("settings-out");
        settingsContainer.AddToClassList("settings-in");
    }

    private void HideSettings()
    {
        settingsContainer.RemoveFromClassList("settings-in");
        settingsContainer.AddToClassList("settings-out");
    }

    private void NextLevel()
    {
        currentLevel++;

        if (currentLevel >= gameSettingsData.GameLevels.Length)
        {
            currentLevel = 0;
        }

        SetLevel(currentLevel);
    }

    private void PreviousLevel()
    {
        currentLevel--;

        if (currentLevel <= 0)
        {
            currentLevel = gameSettingsData.GameLevels.Length - 1;
        }

        SetLevel(currentLevel);
    }

    private void SetLevel(int level)
    {
        levelText.text = gameSettingsData.GameLevels[level].LevelName;
        gameSettingsData.SetCurrentLevel(level);
    }
}
