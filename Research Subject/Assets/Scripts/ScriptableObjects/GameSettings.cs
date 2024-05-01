using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettingsSO", order = 1)]
public class GameSettings : ScriptableObject
{
    public int micIndex;
    public float micSensitivity = 0.5f;
    public float volume = 1f;
}
