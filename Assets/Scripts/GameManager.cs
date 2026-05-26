using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private GameSettingsData gameSettingsData;

    private void Awake()
    {
        Physics.gravity = gameSettingsData.Gravity;
    }
}
