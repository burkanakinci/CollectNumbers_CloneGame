using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{

    #region Datas
    public int LevelNumber;
    public int MovesCount;
    public int GridRowCount;
    public int GridColumnCount;
    public List<MatchableOnCell> LevelMatchables;
    public TargetMatchable[] TargetMatchables;
    #endregion
}
