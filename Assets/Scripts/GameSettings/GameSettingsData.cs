using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Game/GameSettingsData")]
public class GameSettingsData : ScriptableObject
{
    public Action OnLevelSelected { get; set; }
    public Action OnPaperVisualChanged { get; set; }
    public int SelectedLevel { get; private set; }
    public int SelectedPaperVisual { get; private set; }

    public Vector3 Gravity = new Vector3(0.0f, -9.81f, 1.0f);
    public LevelData[] GameLevels;
    public GameObject[] PaperVisuals;

    public void SetCurrentLevel(int level)
    {
        if (level < GameLevels.Length && level >= 0)
        {
            SelectedLevel = level;
            OnLevelSelected?.Invoke();
        }
    }

    public void SetCurrentPaperVisual()
    {
        SelectedPaperVisual++;

        if (SelectedPaperVisual >= PaperVisuals.Length)
        {
            SelectedPaperVisual = 0;
        }

        OnPaperVisualChanged?.Invoke();
    }
}
