#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Linq;

public class LevelDataCreator : MonoBehaviour
{

    [HideInInspector] public LevelData TempLevelData;
    private string m_SavePath;
    [SerializeField] private Sprite[] m_SpawnedSprite;

    #region  LevelDataFields
    [HideInInspector] public int LevelNumber;
    [HideInInspector] public int MovesCount;
    [HideInInspector] public MatchableColor[,] MatchableTypes;
    [HideInInspector] public int GridCellXCount = 9, GridCellYCount = 9;
    private List<Target> m_TargetMatchables = new List<Target>();
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
        TempLevelData.CameraOrtographicSize = Camera.main.orthographicSize;
        TempLevelData.CameraPosition = Camera.main.transform.position;
        TempLevelData.LevelMatchables = new List<MatchableOnCell>();
        for (int _horizontalCount = MatchableTypes.GetLength(0) - 1; _horizontalCount >= 0; _horizontalCount--)
        {
            for (int _verticalCount = MatchableTypes.GetLength(1) - 1; _verticalCount >= 0; _verticalCount--)
            {
                TempLevelData.LevelMatchables.Add(new MatchableOnCell
                {
                    MatchableColorOnCell = MatchableTypes[_horizontalCount, _verticalCount],
                    MatchableXIndis = _horizontalCount,
                    MatchableYIndis = _verticalCount
                });
            }
        }
        m_TargetMatchables = GameObject.FindObjectsOfType<Target>().ToList();
        TempLevelData.TargetMatchables = new TargetMatchable[m_TargetMatchables.Count];
        for (int _targetCount = 0; _targetCount < m_TargetMatchables.Count; _targetCount++)
        {
            TempLevelData.TargetMatchables[_targetCount].TargetMatchableColor = m_TargetMatchables[_targetCount].CurrentTargetMatchable.TargetMatchableColor;
            TempLevelData.TargetMatchables[_targetCount].TargetMatchableCount = m_TargetMatchables[_targetCount].CurrentTargetMatchable.TargetMatchableCount;
            TempLevelData.TargetMatchables[_targetCount].TargetMatchablePosition = m_TargetMatchables[_targetCount].transform.position;
        }

        AssetDatabase.CreateAsset(TempLevelData, m_SavePath);
        AssetDatabase.SaveAssets();
    }
    private Transform m_TempSpawnedMatchable;
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
                m_TempSpawnedMatchable = Instantiate(
                    m_MatchablePrefab,
                    new Vector3(_horizontalCount, _verticalCount, 0.0f),
                    Quaternion.identity,
                    m_MatchablesParent
                ).transform;
                m_TempSpawnedMatchable.GetChild(0).GetChild(0).transform.GetComponent<SpriteRenderer>().sprite = m_SpawnedSprite[(int)MatchableTypes[_horizontalCount, _verticalCount]];
            }
        }

        Camera.main.transform.position = new Vector3(
            ((MatchableTypes.GetLength(0) - 1) * 0.5f),
            ((MatchableTypes.GetLength(1) - 1) * 0.5f),
            Camera.main.transform.position.z
        );
    }
}
#endif