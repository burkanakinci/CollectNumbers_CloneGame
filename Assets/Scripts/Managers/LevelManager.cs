using System;
using System.Linq;
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
    public Vector3 CurrentCameraPosition => m_CurrentLevelData.CameraPosition;
    public float CurrentCameraSize => m_CurrentLevelData.CameraOrtographicSize;
    public int CurrentRowCount => m_CurrentLevelData.GridRowCount;
    public int CurrentColumnCount => m_CurrentLevelData.GridColumnCount;
    public List<TargetMatchable> CurrentTargetMatchables { get; private set; }
    public int RemainingMoveCount { get; private set; }
    #endregion

    #region Actions
    public event Action OnCleanSceneObject;
    public event Action<int> OnChangeMoveCount;
    #endregion
    public override void Initialize()
    {
        m_MaxLevelDataCount = Resources.LoadAll("LevelDatas", typeof(LevelData)).Length;
        GameManager.Instance.InputManager.OnClicked += DecreaseMoveCount;
        CurrentTargetMatchables = new List<TargetMatchable>();
    }
    public void SetLevelNumber(int _levelNumber)
    {
        m_CurrentLevelNumber = _levelNumber;
        m_ActiveLevelDataNumber = (m_CurrentLevelNumber <= m_MaxLevelDataCount) ? (m_CurrentLevelNumber) : ((int)(UnityEngine.Random.Range(1, (m_MaxLevelDataCount + 1))));
    }
    public void SetTargetMatchableList()
    {
        CurrentTargetMatchables.Clear();
        CurrentTargetMatchables.AddRange(m_CurrentLevelData.TargetMatchables.ToList());
    }
    public void CreateLevel()
    {
        ChangeMoveCount(m_CurrentLevelData.MovesCount);
        CleanSceneObject();
        StartSpawnSceneCoroutine();
    }
    public void CleanSceneObject()
    {
        OnCleanSceneObject?.Invoke();
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
        m_TempSpawnedPos.y = GameManager.Instance.CameraManager.CameraUpEndPos;
        for (int _matchableCount = 0; _matchableCount < m_CurrentLevelData.LevelMatchables.Count; _matchableCount++)
        {
            m_TempSpawnedPos.x = m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableXIndis;
            m_TempSpawnedMatchable = GameManager.Instance.ObjectPool.SpawnFromPool(
                PooledObjectType.Matchable,
               m_TempSpawnedPos,
                Quaternion.identity,
                GameManager.Instance.Entities.GetActiveParent(ActiveParents.MatchableActiveParent)
            ).GetGameObject().GetComponent<Matchable>();
            m_TempSpawnedMatchable.SetMatchableCurrentNode(GameManager.Instance.GridManager.GetNode(m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableXIndis, m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableYIndis));
            if (m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableColorOnCell == MatchableColor.Random)
            {
                m_TempSpawnedMatchable.SetMatchableType(GameManager.Instance.Entities.GetMatchableType(UnityEngine.Random.Range(0, ((int)MatchableColor.Random))));
            }
            else
            {
                m_TempSpawnedMatchable.SetMatchableType(GameManager.Instance.Entities.GetMatchableType((int)(m_CurrentLevelData.LevelMatchables[_matchableCount].MatchableColorOnCell)));
            }
            m_TempSpawnedMatchable.SetMatchableVisual();
        }
        GameManager.Instance.Entities.CheckBlastable();
        StartCheckSpawnCorouitne();
    }
    private Coroutine m_CheckSpawnCorouitine;
    private void StartCheckSpawnCorouitne()
    {
        if (m_CheckSpawnCorouitine != null)
        {
            StopCoroutine(m_CheckSpawnCorouitine);
        }
        m_CheckSpawnCorouitine = StartCoroutine(CheckSpawnCoroutine());
    }
    private IEnumerator CheckSpawnCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if (GameManager.Instance.Entities.BlastableCount > 0)
        {
            CreateLevel();
        }
        else
        {
            GameManager.Instance.Entities.CompleteSpawn();
        }
    }
    #endregion
    private void ChangeMoveCount(int _count)
    {
        RemainingMoveCount = _count;
        OnChangeMoveCount?.Invoke(RemainingMoveCount);
    }
    private void DecreaseMoveCount()
    {
        ChangeMoveCount(RemainingMoveCount - 1);
    }
    public void DecreaseTarget(MatchableColor _type)
    {
        for (int _count = 0; _count < CurrentTargetMatchables.Count; _count++)
        {
            if (CurrentTargetMatchables[_count].TargetMatchableColor == _type)
            {
                CurrentTargetMatchables.Remove(CurrentTargetMatchables[_count]);
                break;
            }
        }
        if (CurrentTargetMatchables.Count == 0)
        {
            GameManager.Instance.LevelSuccess();
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.InputManager.OnClicked -= DecreaseMoveCount;
    }
}