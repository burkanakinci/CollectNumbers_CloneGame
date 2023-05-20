using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Matchable : PooledObject
{
    [SerializeField] private MatchableVisual m_MatchableVisual;
    [SerializeField] private MatchableData m_MatchableData;
    public MatchableType CurrentMatchableType { get; private set; }
    [SerializeField] private Node m_CurrentNode;
    public override void Initialize()
    {
        m_MatchableVisual.Initialize(this);
        m_SpawnSequenceID = GetInstanceID() + "m_SpawnSequenceID";
        m_MatchableMoveTweenID = GetInstanceID() + "m_MatchableMoveTweenID";
        m_StartSpawnSequenceDelayID = GetInstanceID() + "m_StartSpawnSequenceDelayID";
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        m_MatchableVisual.transform.localScale = Vector3.zero;
    }
    public override void OnObjectDeactive()
    {
        base.OnObjectDeactive();
        KillAllTween();
        m_MatchableVisual.KillAllTween();
    }
    public void SetMatchableType(MatchableType _matchableType)
    {
        CurrentMatchableType = _matchableType;
        m_MatchableVisual.SetMatchableVisual(CurrentMatchableType.MatchableColor);
    }
    public void ClickedMatchable()
    {
        if (CurrentMatchableType.MatchableColor == MatchableColor.Random)
        {
            return;
        }
        else
        {
            SetMatchableType(GameManager.Instance.Entities.GetMatchableType((int)CurrentMatchableType.MatchableColor + 1));
        }
    }
    public void SetMatchableCurrentNode(Node _node)
    {
        m_CurrentNode = _node;
    }
    private string m_MatchableMoveTweenID;
    public Tween MoveMatchable(Vector3 _targetPos, float _duration, Ease _ease)
    {
        DOTween.Kill(m_MatchableMoveTweenID);
        return transform.DOMove(_targetPos, _duration)
        .SetEase(_ease)
        .SetId(m_MatchableMoveTweenID);
    }
    private string m_StartSpawnSequenceDelayID;
    private float m_SpawnTweenDelay;
    public void StartSpawnSequence()
    {
        m_SpawnTweenDelay = m_CurrentNode.NodeYIndis * 0.2f;
        m_SpawnTweenDelay += 0.2f;
        DOTween.Kill(m_StartSpawnSequenceDelayID);
        DOVirtual.DelayedCall(m_SpawnTweenDelay, SpawnSequence)
        .SetId(m_StartSpawnSequenceDelayID);
    }
    private string m_SpawnSequenceID;
    private Sequence m_SpawnSequence;
    private void SpawnSequence()
    {
        DOTween.Kill(m_SpawnSequenceID);
        m_SpawnSequence.Append(MoveMatchable(m_CurrentNode.GetNodePosition(), m_MatchableData.SpawnMoveDuration, m_MatchableData.SpawnMoveEase));
        m_SpawnSequence.Join(m_MatchableVisual.MatchableVisualScaleTween(Vector3.one, m_MatchableData.SpawnScaleDuration, m_MatchableData.SpawnScaleEase));
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_SpawnSequenceID);
        DOTween.Kill(m_MatchableMoveTweenID);
        DOTween.Kill(m_StartSpawnSequenceDelayID);
    }
}
