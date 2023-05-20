using UnityEngine;
using System;


public class Entities : CustomBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] private Transform[] m_DeactiveParents;
    [SerializeField] private Transform[] m_ActiveParents;

    [Header("MatchableTypes")]
    [SerializeField] private MatchableType[] m_MatchableTypes;
    public override void Initialize()
    {
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
}