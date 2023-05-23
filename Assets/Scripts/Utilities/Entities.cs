using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;


public class Entities : CustomBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] private Transform[] m_DeactiveParents;
    [SerializeField] private Transform[] m_ActiveParents;

    [Header("MatchableTypes")]
    [SerializeField] private MatchableType[] m_MatchableTypes;

    #region ExternalAccess
    public int BlastableCount => m_BlastedMatchables.Count;
    #endregion

    private List<Matchable> m_BlastedMatchables;

    #region Events
    public event Action OnCompleteSpawn;
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
                }
                break;
            case ListOperations.Substraction:
                if (m_BlastedMatchables.Contains(_matchable))
                {
                    m_BlastedMatchables.Remove(_matchable);
                }
                break;
        }
    }
    public void BlastMatchables()
    {
        for (int _matchables = 0; _matchables < m_BlastedMatchables.Count - 1; _matchables++)
        {
            m_BlastedMatchables[_matchables].BlastMatchable();
        }
    }
    #endregion
    public void CheckBlastable()
    {
        OnCheckBlast?.Invoke();
    }
    public void CompleteSpawn()
    {
        OnCompleteSpawn?.Invoke();
    }
}