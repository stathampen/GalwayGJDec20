using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelRound", menuName = "ScriptableObjects/LevelRoundInstance")]
public class LevelRound : ScriptableObject
{
    public int maxMissesCount;
    public requestedType [] typesWanted;
}

[System.Serializable]
public class requestedType
{
    public string potionName;
    public int potionCount;
}
