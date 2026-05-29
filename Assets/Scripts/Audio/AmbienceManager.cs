using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [SerializeField]
    private GameSettingsData gameSettingsData;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        gameSettingsData.OnLevelSelected += ChangeAmbience;
    }

    private void OnDestroy()
    {
        gameSettingsData.OnLevelSelected -= ChangeAmbience;
    }

    private void ChangeAmbience()
    {
        audioSource.clip = gameSettingsData.GameLevels[gameSettingsData.SelectedLevel].AmbienceClip;
        audioSource.Play();
    }
}
