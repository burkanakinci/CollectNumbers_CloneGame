using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Matchable : PooledObject
{
    [SerializeField] private MatchableVisual m_MatchableVisual;
    [SerializeField] private MatchableData m_MatchableData;
    public MatchableType CurrentMatchableType { get; private set; }
    [SerializeField] private Node m_CurrentNode;
    private Node[] m_NeighbourNodes;

    #region Initialize
    public override void Initialize()
    {
        m_MatchableVisual.Initialize(this);
        m_NeighbourNodes = new Node[4];
        InitializeTween();
    }

    private void InitializeTween()
    {
        m_MatchableMoveTweenID = GetInstanceID() + "m_MatchableMoveTweenID";
        m_StartSpawnSequenceDelayID = GetInstanceID() + "m_StartSpawnSequenceDelayID";
        m_CompleteClickedDelayID = GetInstanceID() + "m_CompleteClickedDelayID";
    }
    #endregion
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        CanBlastable = false;
        m_MatchableVisual.transform.localScale = Vector3.zero;
        GameManager.Instance.Entities.OnCheckBlast += CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn += StartSpawnSequence;
    }
    public override void OnObjectDeactive()
    {
        KillAllTween();
        SetMatchableCurrentNode(null);
        m_MatchableVisual.ResetMatchableVisual();
        GameManager.Instance.Entities.OnCheckBlast -= CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn -= StartSpawnSequence;
        GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Substraction, this);
        base.OnObjectDeactive();
    }
    public void SetMatchableType(MatchableType _matchableType)
    {
        CurrentMatchableType = _matchableType;
    }
    public void SetMatchableVisual()
    {
        m_MatchableVisual.SetVisual(CurrentMatchableType.MatchableColor);
    }
    public void SetMatchableCurrentNode(Node _node)
    {
        if (_node != null)
        {
            m_CurrentNode = _node;
            m_CurrentNode.SetMatchableOnNode(this);
            SetNeighbourNodes();
        }
        else
        {
            m_CurrentNode.SetMatchableOnNode(null);
            m_CurrentNode = _node;
        }
    }
    private void SetNeighbourNodes()
    {
        m_NeighbourNodes[0] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis + 1);
        m_NeighbourNodes[1] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis - 1);
        m_NeighbourNodes[2] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis - 1, m_CurrentNode.NodeYIndis);
        m_NeighbourNodes[3] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis + 1, m_CurrentNode.NodeYIndis);
    }
    private float m_TotalScaleDuration => m_MatchableData.ScaleUpDuration + m_MatchableData.ScaleDownDuration;
    public void ClickedMatchable()
    {
        if (CurrentMatchableType.MatchableColor != MatchableColor.Purple)
        {
            m_MatchableVisual.DissolveTween(m_TotalScaleDuration, Ease.Linear);
            m_MatchableVisual.ScaleTween(
                Vector3.one * m_MatchableData.ClickedMultiplier,
                m_MatchableData.ScaleUpDuration,
                m_MatchableData.ScaleUpEase
            ).
            OnComplete(
                () =>
                {
                    m_MatchableVisual.ScaleTween(
                        Vector3.one,
                        m_MatchableData.ScaleDownDuration,
                        m_MatchableData.ScaleDownEase
                    )
                    .OnComplete(
                        () =>
                        {
                            SetMatchableType(GameManager.Instance.Entities.GetMatchableType((int)CurrentMatchableType.MatchableColor + 1));
                            SetMatchableVisual();
                            CompleteClickedSequence();
                        }
                    );
                }
            );
        }
    }
    private string m_CompleteClickedDelayID;
    private void CompleteClickedSequence()
    {
        DOTween.Kill(m_CompleteClickedDelayID);
        DOVirtual.DelayedCall(0.3f, GameManager.Instance.Entities.CheckGrid);
    }
    private string m_MatchableMoveTweenID;
    public Tween MoveMatchable(Vector3 _targetPos, float _duration, Ease _ease)
    {
        DOTween.Kill(m_MatchableMoveTweenID);
        return transform.DOMove(_targetPos, _duration)
        .SetEase(_ease)
        .SetId(m_MatchableMoveTweenID);
    }
    #region SpawnTween 
    private string m_StartSpawnSequenceDelayID;
    private float m_SpawnTweenDelay;
    public void StartSpawnSequence(bool _useMoveTween)
    {
        m_SpawnTweenDelay = 0.2f;
        m_SpawnTweenDelay += m_CurrentNode.NodeYIndis * 0.05f;
        m_SpawnTweenDelay += m_CurrentNode.NodeXIndis * 0.05f;
        DOTween.Kill(m_StartSpawnSequenceDelayID);
        DOVirtual.DelayedCall(m_SpawnTweenDelay, () => { SpawnSequence(_useMoveTween); })
        .SetId(m_StartSpawnSequenceDelayID);
    }
    private void SpawnSequence(bool _useMoveTween)
    {
        if (_useMoveTween)
        {
            MoveMatchable(m_CurrentNode.GetNodePosition(), m_MatchableData.SpawnMoveDuration, m_MatchableData.SpawnMoveEase);
        }
        else
        {
            transform.position = m_CurrentNode.GetNodePosition();
        }
        m_MatchableVisual.ScaleTween(Vector3.one, m_MatchableData.SpawnScaleDuration, m_MatchableData.SpawnScaleEase)
        .OnComplete(() =>
        {
            if (!_useMoveTween)
                m_MatchableVisual.PunchTween(m_MatchableData.PunchMultiplier, m_MatchableData.SpawnPunchDuration, m_MatchableData.SpawnPunchMoveEase);
        });
    }
    #endregion
    #region CheckBlastable
    public bool CanBlastable { get; private set; }

    public void CheckBlastable()
    {
        CanBlastable = false;
        if (m_NeighbourNodes[(int)NeighbourType.Up] != null && m_NeighbourNodes[(int)NeighbourType.Down] != null)
        {
            if (m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode != null && m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode != null)
            {
                if ((m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor || m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode.CurrentMatchableType.MatchableColor == MatchableColor.Random)
                 && (m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor || m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode.CurrentMatchableType.MatchableColor == MatchableColor.Random))
                {
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode);
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode);
                    CanBlastable = true;
                }
            }
        }
        if (m_NeighbourNodes[(int)NeighbourType.Left] != null && m_NeighbourNodes[(int)NeighbourType.Right] != null)
        {
            if (m_NeighbourNodes[(int)NeighbourType.Left].MatchableOnNode != null && m_NeighbourNodes[(int)NeighbourType.Right].MatchableOnNode != null)
            {
                if (m_NeighbourNodes[(int)NeighbourType.Left].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor && m_NeighbourNodes[(int)NeighbourType.Right].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor)
                {
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Left].MatchableOnNode);
                    GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Right].MatchableOnNode);
                    CanBlastable = true;
                }
            }
        }
    }
    #endregion
    private Vector3 m_TargetMovementPos;
    public void TargetSequence(Target _target)
    {
        m_TargetMovementPos = _target.BlastMatchablePos;
        m_TargetMovementPos.z = transform.position.z;
        KillAllTween();
        MoveMatchable(
            m_TargetMovementPos,
            m_MatchableData.TargetMovementDuration,
            m_MatchableData.TargetMovementEase
        ).OnComplete(() =>
        {
            GameManager.Instance.ObjectPool.SpawnFromPool(PooledObjectType.Blast_VFX, m_TargetMovementPos, Quaternion.identity, GameManager.Instance.Entities.GetActiveParent(ActiveParents.VFXActiveParent));
            _target.DecreaseTarget();
            OnObjectDeactive();
        });
        m_MatchableVisual.ScaleTween(
            Vector3.zero,
            m_MatchableData.TargetMovementDuration,
            m_MatchableData.TargetMovementEase
        );
    }
    public void BlastMatchable()
    {
        m_MatchableVisual.ScaleTween(Vector3.zero, m_MatchableData.BlastTweenDuration, m_MatchableData.BlastTweenEase)
        .OnComplete(() =>
        {
            GameManager.Instance.ObjectPool.SpawnFromPool(
            PooledObjectType.Blast_VFX,
            transform.position,
            Quaternion.identity,
            GameManager.Instance.Entities.GetActiveParent(ActiveParents.VFXActiveParent));
            OnObjectDeactive();
        });
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_CompleteClickedDelayID);
        DOTween.Kill(m_MatchableMoveTweenID);
        DOTween.Kill(m_StartSpawnSequenceDelayID);
    }
    private void OnDestroy()
    {
        KillAllTween();
        m_MatchableVisual.ResetMatchableVisual();
    }
}
