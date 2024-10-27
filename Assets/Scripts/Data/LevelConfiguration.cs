using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "LevelData/LevelConfiguration", order = 1)]
public class LevelConfiguration : ScriptableObject
{
    public LevelInfo[] levelInfo;
}

[Serializable]
public class LevelInfo
{
    public int levelNumber;
    public int rows;
    public int columns;
}
