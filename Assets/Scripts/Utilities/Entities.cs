using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class Entities : CustomBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] private Transform[] m_DeactiveParents;
    [SerializeField] private Transform[] m_ActiveParents;
    [SerializeField] private Transform m_CoinPos;

    [Header("MatchableTypes")]
    [SerializeField] private MatchableType[] m_MatchableTypes;

    [Header("Finish BG")]
    [SerializeField] private Sprite[] m_FinishBG;

    #region ExternalAccess
    public int BlastableCount => m_BlastedMatchables.Count;
    public Vector3 CoinScorePos => m_CoinPos.position;
    #endregion

    private List<Matchable> m_BlastedMatchables;

    #region Events
    public event Action<bool> OnCompleteSpawn;
    public event Action OnBlastMatchables;
    public event Action<Matchable> OnAddedBlastMatchable;
    public event Action OnCheckBlast;
    #endregion
    public override void Initialize()
    {
        m_BlastedMatchables = new List<Matchable>();
    }
    #region  Getter
    public Transform GetActiveParent(ActiveParents _activeParent)
    {
        return m_ActiveParents[(int)_activeParent];
    }
    public Transform GetDeactiveParent(DeactiveParents _deactiveParent)
    {
        return m_DeactiveParents[(int)_deactiveParent];
    }
    public MatchableType GetMatchableType(int _color)
    {
        return m_MatchableTypes[_color];
    }
    public Sprite GetFinishBG(FinishAreaType _finishType)
    {
        return m_FinishBG[(int)_finishType];
    }
    #endregion
    #region Setter
    public void SetBlastedMatchables(ListOperations _operation, Matchable _matchable)
    {
        switch (_operation)
        {
            case ListOperations.Adding:
                if (!m_BlastedMatchables.Contains(_matchable))
                {
                    m_BlastedMatchables.Add(_matchable);
                    OnBlastMatchables += _matchable.BlastMatchable;
                    OnAddedBlastMatchable?.Invoke(_matchable);
                }
                break;
            case ListOperations.Substraction:
                if (m_BlastedMatchables.Contains(_matchable))
                {
                    m_BlastedMatchables.Remove(_matchable);
                    OnBlastMatchables -= _matchable.BlastMatchable;
                }
                break;
        }
    }
    #endregion
    public void CheckGrid()
    {
        CheckBlastable();
        BlastMatchables();
    }
    public void CheckBlastable()
    {
        OnCheckBlast?.Invoke();
    }
    private Coroutine m_BlastMatchablesCoroutine;
    private void BlastMatchables()
    {
        if (m_BlastMatchablesCoroutine != null)
        {
            StopCoroutine(m_BlastMatchablesCoroutine);
        }
        m_BlastMatchablesCoroutine = StartCoroutine(BlastMatchablesCoroutine());
    }
    private IEnumerator BlastMatchablesCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.35f);
        if (m_BlastedMatchables.Count == 0)
        {
            if (GameManager.Instance.LevelManager.RemainingMoveCount <= 0)
                GameManager.Instance.LevelFailed();
            else
                GameManager.Instance.InputManager.SetCanClickable(true);
        }
        else
        {
            OnBlastMatchables?.Invoke();
            GameManager.Instance.GridManager.StartFillEmptyNodes();
        }
    }
    public void CompleteSpawn()
    {
        OnCompleteSpawn?.Invoke(false);
    }
}