using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStatus", menuName = "ScriptableObjects/GameStatusSO", order = 1)]
public class GameStatus : ScriptableObject
{
    public bool initialized = false;
}
