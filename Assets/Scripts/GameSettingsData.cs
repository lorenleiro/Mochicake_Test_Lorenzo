using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsData", menuName = "Game/GameSettingsData")]
public class GameSettingsData : ScriptableObject
{
    public Vector3 Gravity = new Vector3(0.0f, -9.81f, 1.0f);
}
