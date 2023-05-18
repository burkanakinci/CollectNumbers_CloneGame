
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;



[CustomEditor(typeof(LevelDataCreator))]
public class LevelCreatorEditor : Editor
{
    private MatchableType[,] m_TempMatchableTypes = new MatchableType[9, 9];
    private MatchableType m_TempTargetMatchableType = MatchableType.Random;
    private int m_TempTargetMatchableCount = 1;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelDataCreator m_LevelDataCreator = (LevelDataCreator)target;

        #region Moves Count Level Number Area
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(25f);

        GUILayout.Label("Moves Count");
        m_LevelDataCreator.MovesCount = EditorGUILayout.IntField(m_LevelDataCreator.MovesCount);

        GUILayout.Space(25);

        GUILayout.Label("Level Number");
        m_LevelDataCreator.LevelNumber = EditorGUILayout.IntField(m_LevelDataCreator.LevelNumber);

        if (EditorGUI.EndChangeCheck())
        {
        }
        #endregion

        GUILayout.Space(25);

        m_TempTargetMatchableCount = EditorGUILayout.IntField("Target Matchable Count", m_TempTargetMatchableCount);
        m_TempTargetMatchableType = (MatchableType)EditorGUILayout.EnumPopup("Target Matchable Type", m_TempTargetMatchableType);



        if (GUILayout.Button("Add Target Matchable", GUILayout.Width(150), GUILayout.Height(45)))
        {
            m_LevelDataCreator.TargetMatchables.Add(new TargetMatchable
            {
                TargetMatchableType = m_TempTargetMatchableType,
                TargetMatchableCount = m_TempTargetMatchableCount
            });
        }

        EditorGUI.BeginChangeCheck();

        GUILayout.Space(25f);

        GUILayout.Label("Grid Cell Width Count");
        m_LevelDataCreator.GridCellXCount = EditorGUILayout.IntSlider(m_LevelDataCreator.GridCellXCount, 4, 9);
        GUILayout.Label("Grid Cell Height Count");
        m_LevelDataCreator.GridCellYCount = EditorGUILayout.IntSlider(m_LevelDataCreator.GridCellYCount, 4, 9);

        GUILayout.Space(25f);

        if (EditorGUI.EndChangeCheck())
        {
            m_TempMatchableTypes = new MatchableType[m_LevelDataCreator.GridCellXCount, m_LevelDataCreator.GridCellYCount];

            m_LevelDataCreator.MatchableTypes = m_TempMatchableTypes;
            m_LevelDataCreator.SpawnMatchables();
        }

        EditorGUI.BeginChangeCheck();
        GUILayout.Label("Matchable On Grid");
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        for (int _verticalCellSize = m_LevelDataCreator.GridCellYCount - 1; _verticalCellSize >= 0; _verticalCellSize--)
        {
            for (int _horizontalCellSize = 0; _horizontalCellSize < m_LevelDataCreator.GridCellXCount; _horizontalCellSize++)
            {
                m_TempMatchableTypes[_horizontalCellSize, _verticalCellSize] =
                    (MatchableType)EditorGUILayout.EnumPopup
                    ("", m_TempMatchableTypes[_horizontalCellSize, _verticalCellSize],
                    GUILayout.Width(75), GUILayout.Height(45));
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();


        if (EditorGUI.EndChangeCheck())
        {
            m_LevelDataCreator.MatchableTypes = m_TempMatchableTypes;
            m_LevelDataCreator.SpawnMatchables();
        }

        if (GUILayout.Button("CreateLevel"))
        {
            m_LevelDataCreator.CreateLevel();
        }
    }

}
#endif

