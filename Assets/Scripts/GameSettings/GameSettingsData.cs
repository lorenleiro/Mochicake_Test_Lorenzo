using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Game/GameSettingsData")]
public class GameSettingsData : ScriptableObject
{
    public Action OnLevelSelected { get; set; }
    public int SelectedLevel { get; private set; }

    public Vector3 Gravity = new Vector3(0.0f, -9.81f, 1.0f);
    public LevelData[] GameLevels;

    public void SetCurrentLevel(int level)
    {
        if (level < GameLevels.Length && level >= 0)
        {
            SelectedLevel = level;
            OnLevelSelected?.Invoke();
        }
    }
}
