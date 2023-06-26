using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[CreateAssetMenu(fileName = "CoinData", menuName = "Coin Data")]
public class CoinData : ScriptableObject
{
    #region Datas
    [Header("Spawn Punch")]
    [SerializeField] private float m_SpawnPunchMultiplier;
    [SerializeField] private float m_SpawnPunchDuration;
    [SerializeField] private Ease m_SpawnPunchEase;

    [Header("Spawn Punch Down")]
    [SerializeField] private float m_SpawnPuncDownDuration;
    [SerializeField] private Ease m_SpawnPunchDownEase;

    [Header("Score Movement")]
    [SerializeField] private float m_MoveDuration;
    [SerializeField] private Ease m_MoveEase;
    #endregion
    #region ExternalAccess
    public Vector3 SpawnPunchScale => Vector3.one * m_SpawnPunchMultiplier;
    public float SpawnPunchDuration => m_SpawnPunchDuration;
    public Ease SpawnPunchEase => m_SpawnPunchEase;
    public float SpawnPuncDownDuration => m_SpawnPuncDownDuration;
    public Ease SpawnPunchDownEase => m_SpawnPunchDownEase;
    public float MoveDuration => m_MoveDuration;
    public Ease MoveEase => m_MoveEase;
    #endregion
}
