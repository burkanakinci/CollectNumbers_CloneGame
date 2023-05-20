using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[CreateAssetMenu(fileName = "MatchableData", menuName = "Matchable Data")]
public class MatchableData : ScriptableObject
{

    #region Datas

    [Header("Spawn Movement")]
    [SerializeField] private float m_SpawnMoveDuration;
    [SerializeField] private Ease m_SpawnMoveEase;

    [Header("Spawn Scale")]
    [SerializeField] private float m_SpawnScaleDuration;
    [SerializeField] private Ease m_SpawnScaleEase;

    #endregion
    #region ExternalAccess
    public float SpawnMoveDuration => m_SpawnMoveDuration;
    public Ease SpawnMoveEase => m_SpawnMoveEase;
    public float SpawnScaleDuration => m_SpawnScaleDuration;
    public Ease SpawnScaleEase => m_SpawnScaleEase;
    #endregion
}
