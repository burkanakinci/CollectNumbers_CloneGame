
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : CustomBehaviour
{
    #region Fields
    private LevelData m_CurrentLevelData;
    private int m_CurrentLevelNumber;
    private int m_ActiveLevelDataNumber;
    private int m_MaxLevelDataCount;
    #endregion

    #region ExternalAccess
    public int CurrentRowCount => m_CurrentLevelData.GridRowCount;
    public int CurrentColumnCount => m_CurrentLevelData.GridColumnCount;
    #endregion

    #region Actions
    public event Action OnCleanSceneObject;
    #endregion
    public override void Initialize()
    {
        m_MaxLevelDataCount = Resources.LoadAll("LevelDatas", typeof(LevelData)).Length;
    }
    public void SetLevelNumber(int _levelNumber)
    {
        m_CurrentLevelNumber = _levelNumber;
        m_ActiveLevelDataNumber = (m_CurrentLevelNumber <= m_MaxLevelDataCount) ? (m_CurrentLevelNumber) : ((int)(UnityEngine.Random.Range(1, (m_MaxLevelDataCount + 1))));
    }
    public void CreateLevel()
    {
        OnCleanSceneObject?.Invoke();
        StartSpawnSceneCoroutine();
    }
    private Coroutine m_SpawnSceneCoroutine;
    private void StartSpawnSceneCoroutine()
    {
        if (m_SpawnSceneCoroutine != null)
        {
            StopCoroutine(m_SpawnSceneCoroutine);
        }
        m_SpawnSceneCoroutine = StartCoroutine(SpawnSceneCoroutine());
    }
    private IEnumerator SpawnSceneCoroutine()
    {
        yield return new WaitForEndOfFrame();
        SpawnSceneObjects();
    }
    public void GetLevelData()
    {
        m_CurrentLevelData = Resources.Load<LevelData>("LevelDatas/" + m_ActiveLevelDataNumber + "LevelData");
    }
    private void SpawnSceneObjects()
    {
        SpawnMatchables();
    }

    #region Spawn Scene Objects
    private Color m_TempSpawnedMatchableColor;
    private Matchable m_TempSpawnedMatchable;
    private Vector3 m_TempSpawnedPos = Vector3.zero;
    private void SpawnMatchables()
    {
        m_TempSpawnedPos.y = GameManager.Instance.CameraManager.CameraSize + GameManager.Instance.CameraManager.CameraPos.y;
        for (int _matchableCount = 0; _matchableCount < m_CurrentLevelData.LevelMatchables.Count; _matchableCount++)
        {
            m_TempSpawnedPos.x = m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableXIndis;
            m_TempSpawnedMatchable = GameManager.Instance.ObjectPool.SpawnFromPool(
                PooledObjectType.Matchable,
               m_TempSpawnedPos,
                Quaternion.identity,
                GameManager.Instance.Entities.GetActiveParent(ActiveParents.MatchableActiveParent)
            ).GetGameObject().GetComponent<Matchable>();
            m_TempSpawnedMatchable.SetMatchableType(GameManager.Instance.Entities.GetMatchableType((int)(m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableColorOnCell)));
            m_TempSpawnedMatchable.SetMatchableCurrentNode(GameManager.Instance.GridManager.GetNode(m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableXIndis, m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableYIndis));
            m_TempSpawnedMatchable.StartSpawnSequence();
        }
    }
    #endregion
}