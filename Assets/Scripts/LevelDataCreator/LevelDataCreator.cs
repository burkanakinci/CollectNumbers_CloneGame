using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class LevelDataCreator : MonoBehaviour
{

    [HideInInspector] public LevelData TempLevelData;
    private string m_SavePath;

    #region  LevelDataFields
    [HideInInspector] public int LevelNumber;
    [HideInInspector] public int MovesCount;
    [HideInInspector] public MatchableType[,] MatchableTypes;
    [HideInInspector] public int GridCellXCount = 9, GridCellYCount = 9;
    public List<TargetMatchable> TargetMatchables = new List<TargetMatchable>();
    #endregion
    #region SceneFields
    [SerializeField] private Transform m_MatchablesParent;
    [SerializeField] private GameObject m_MatchablePrefab;
    #endregion
    public void CreateLevel()
    {
        AssetDatabase.DeleteAsset("Assets/Resources/LevelDatas/" + LevelNumber + "LevelData.asset");
        AssetDatabase.Refresh();

        TempLevelData = ScriptableObject.CreateInstance<LevelData>();
        m_SavePath = AssetDatabase.GenerateUniqueAssetPath("Assets/Resources/LevelDatas/" + LevelNumber + "LevelData.asset");

        TempLevelData.LevelNumber = LevelNumber;
        TempLevelData.MovesCount = MovesCount;
        TempLevelData.GridRowCount = GridCellXCount;
        TempLevelData.GridColumnCount = GridCellYCount;
        TempLevelData.LevelMatchables = new MatchableOnCell[MatchableTypes.GetLength(0), MatchableTypes.GetLength(1)];
        for (int _horizontalCount = MatchableTypes.GetLength(0) - 1; _horizontalCount >= 0; _horizontalCount--)
        {
            for (int _verticalCount = MatchableTypes.GetLength(1) - 1; _verticalCount >= 0; _verticalCount--)
            {
                TempLevelData.LevelMatchables[_horizontalCount, _verticalCount] = new MatchableOnCell
                {
                    MatchableTypeOnCell = MatchableTypes[_horizontalCount, _verticalCount],
                    MatchableXIndis = _horizontalCount,
                    MatchableYIndis = _verticalCount
                };
            }
        }
        TempLevelData.TargetMatchables = TargetMatchables.ToArray();

        AssetDatabase.CreateAsset(TempLevelData, m_SavePath);
        AssetDatabase.SaveAssets();
    }
    private Color m_TempSpawnedColor;
    public void SpawnMatchables()
    {
        for (int _childCount = m_MatchablesParent.childCount - 1; _childCount >= 0; _childCount--)
        {
            DestroyImmediate(m_MatchablesParent.GetChild(_childCount).gameObject);
        }

        for (int _horizontalCount = MatchableTypes.GetLength(0) - 1; _horizontalCount >= 0; _horizontalCount--)
        {
            for (int _verticalCount = MatchableTypes.GetLength(1) - 1; _verticalCount >= 0; _verticalCount--)
            {
                if (MatchableTypes[_horizontalCount, _verticalCount] != MatchableType.Random)
                {
                    ColorUtility.TryParseHtmlString(Colors.ColorArray[(int)MatchableTypes[_horizontalCount, _verticalCount]], out m_TempSpawnedColor);
                }
                else
                {
                    m_TempSpawnedColor = Color.white;
                }

                Instantiate(
                    m_MatchablePrefab,
                    new Vector3(_horizontalCount, _verticalCount, 0.0f),
                    Quaternion.identity,
                    m_MatchablesParent
                ).transform.GetChild(0).GetChild(0).transform.GetComponent<SpriteRenderer>().color = m_TempSpawnedColor;
            }
        }

        Camera.main.transform.position = new Vector3(
            ((MatchableTypes.GetLength(0) - 1) * 0.5f),
            ((MatchableTypes.GetLength(1) - 1) * 0.5f),
            Camera.main.transform.position.z
        );
    }
}


