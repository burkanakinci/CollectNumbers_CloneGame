
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;



[CustomEditor(typeof(LevelDataCreator))]
public class LevelCreatorEditor : Editor
{
    private MatchableColor[,] m_TempMatchableTypes = new MatchableColor[9, 9];
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

        EditorGUI.BeginChangeCheck();

        GUILayout.Space(25f);

        GUILayout.Label("Grid Cell Width Count");
        m_LevelDataCreator.GridCellXCount = EditorGUILayout.IntSlider(m_LevelDataCreator.GridCellXCount, 4, 9);
        GUILayout.Label("Grid Cell Height Count");
        m_LevelDataCreator.GridCellYCount = EditorGUILayout.IntSlider(m_LevelDataCreator.GridCellYCount, 4, 9);

        GUILayout.Space(25f);

        if (EditorGUI.EndChangeCheck())
        {
            SetCameraOrtographicSize(m_LevelDataCreator.GridCellXCount, m_LevelDataCreator.GridCellYCount);
            m_TempMatchableTypes = new MatchableColor[m_LevelDataCreator.GridCellXCount, m_LevelDataCreator.GridCellYCount];
            for (int _verticalCellSize = m_LevelDataCreator.GridCellYCount - 1; _verticalCellSize >= 0; _verticalCellSize--)
            {
                for (int _horizontalCellSize = 0; _horizontalCellSize < m_LevelDataCreator.GridCellXCount; _horizontalCellSize++)
                {
                    m_TempMatchableTypes[_horizontalCellSize, _verticalCellSize] = MatchableColor.Random;
                }
            }
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
                    (MatchableColor)EditorGUILayout.EnumPopup
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
            SetCameraOrtographicSize(m_LevelDataCreator.GridCellXCount, m_LevelDataCreator.GridCellYCount);
            m_LevelDataCreator.MatchableTypes = m_TempMatchableTypes;
            m_LevelDataCreator.SpawnMatchables();
        }

        if (GUILayout.Button("CreateLevel"))
        {
            m_LevelDataCreator.CreateLevel();
        }
    }
    private float m_TempsCameraSize;
    private void SetCameraOrtographicSize(int _xSize, int _ySize)
    {
        if (_ySize > _xSize)
        {
            m_TempsCameraSize = 5.0f;
            m_TempsCameraSize += (_ySize - 4) / 2.0f;
        }
        else
        {
            m_TempsCameraSize = _xSize + 1;
        }
        Camera.main.orthographicSize = m_TempsCameraSize;
    }
}
#endif

