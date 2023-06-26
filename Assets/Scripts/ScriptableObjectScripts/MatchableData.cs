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

    [Header("Spawn Punch")]
    [SerializeField] private float m_PunchMultiplier;
    [SerializeField] private float m_SpawnPunchDuration;
    [SerializeField] private Ease m_SpawnPunchMoveEase;

    [Header("Spawn Scale")]
    [SerializeField] private float m_SpawnScaleDuration;
    [SerializeField] private Ease m_SpawnScaleEase;

    [Header("Blast Tween")]
    [SerializeField] private float m_BlastTweenDuration;
    [SerializeField] private Ease m_BlastTweenEase;

    [Header("Target Movement")]
    [SerializeField] private float m_TargetMovementDuration;
    [SerializeField] private Ease m_TargetMovementEase;

    [Header("Clicked Scale")]
    [SerializeField] private float m_ClickedMultiplier;
    [SerializeField] private float m_ScaleUpDuration;
    [SerializeField] private float m_ScaleDownDuration;
    [SerializeField] private Ease m_ScaleUpEase;
    [SerializeField] private Ease m_ScaleDownEase;

    #endregion
    #region ExternalAccess
    public float SpawnMoveDuration => m_SpawnMoveDuration;
    public Ease SpawnMoveEase => m_SpawnMoveEase;
    public float SpawnScaleDuration => m_SpawnScaleDuration;
    public Ease SpawnScaleEase => m_SpawnScaleEase;
    public float ClickedMultiplier => m_ClickedMultiplier;
    public float ScaleUpDuration => m_ScaleUpDuration;
    public float ScaleDownDuration => m_ScaleDownDuration;
    public Ease ScaleUpEase => m_ScaleUpEase;
    public Ease ScaleDownEase => m_ScaleDownEase;
    public float BlastTweenDuration => m_BlastTweenDuration;
    public Ease BlastTweenEase => m_BlastTweenEase;
    public float TargetMovementDuration => m_TargetMovementDuration;
    public Ease TargetMovementEase => m_TargetMovementEase;
    public float PunchMultiplier => m_PunchMultiplier;
    public float SpawnPunchDuration => m_SpawnPunchDuration;
    public Ease SpawnPunchMoveEase => m_SpawnPunchMoveEase;
    #endregion
}
