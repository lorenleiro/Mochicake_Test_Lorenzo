using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameSettingsData gameSettingsData;

    [SerializeField]
    private GameObject level;

    [SerializeField]
    private Transform levelPivot;


    private void Start()
    {
        gameSettingsData.OnLevelSelected += ChangeLevel;
    }

    private void OnDestroy()
    {
        gameSettingsData.OnLevelSelected -= ChangeLevel;
    }

    private void ChangeLevel()
    {
        if (level != null)
        {
            Destroy(level);
        }

        level = Instantiate(gameSettingsData.GameLevels[gameSettingsData.SelectedLevel].LevelPrefab);
        level.transform.position = levelPivot.transform.position;
    }
}
