using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameSettingsData gameSettingsData;

    [SerializeField]
    [Tooltip("Current loaded visual level.")]
    private GameObject level;

    [SerializeField]
    [Tooltip("Point where the level will be loaded.")]
    private Transform levelPivot;

    private void Start()
    {
        gameSettingsData.OnLevelSelected += ChangeLevel;
    }

    private void OnDestroy()
    {
        gameSettingsData.OnLevelSelected -= ChangeLevel;
    }

    /// <summary>
    /// Recreated the level visual using the one selected on the Game Settings.
    /// </summary>
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
