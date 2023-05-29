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
    private Node[] m_NeighbourNodes;
    public override void Initialize()
    {
        m_MatchableVisual.Initialize(this);
        m_NeighbourNodes = new Node[4];
        m_SpawnSequenceID = GetInstanceID() + "m_SpawnSequenceID";
        m_MatchableMoveTweenID = GetInstanceID() + "m_MatchableMoveTweenID";
        m_StartSpawnSequenceDelayID = GetInstanceID() + "m_StartSpawnSequenceDelayID";
        m_CompleteClickedDelayID = GetInstanceID() + "m_CompleteClickedDelayID";
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        m_MatchableVisual.transform.localScale = Vector3.zero;
        GameManager.Instance.Entities.OnCheckBlast += CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn += StartSpawnSequence;
    }
    public override void OnObjectDeactive()
    {
        KillAllTween();
        m_CurrentNode.SetMatchableOnNode(null);
        m_MatchableVisual.ResetMatchableVisual();
        GameManager.Instance.Entities.OnCheckBlast -= CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn -= StartSpawnSequence;
        GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Substraction, this);
        base.OnObjectDeactive();
    }
    public void SetMatchableType(MatchableType _matchableType)
    {
        CurrentMatchableType = _matchableType;
        m_MatchableVisual.SetMatchableVisual(CurrentMatchableType.MatchableColor);
    }
    public void SetMatchableCurrentNode(Node _node)
    {
        m_CurrentNode = _node;
        m_CurrentNode.SetMatchableOnNode(this);
        SetNeighbourNodes();
    }
    private void SetNeighbourNodes()
    {
        m_NeighbourNodes[0] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis + 1);
        m_NeighbourNodes[1] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis - 1);
        m_NeighbourNodes[2] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis - 1, m_CurrentNode.NodeYIndis);
        m_NeighbourNodes[3] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis + 1, m_CurrentNode.NodeYIndis);
    }
    public void ClickedMatchable()
    {
        if (CurrentMatchableType.MatchableColor != MatchableColor.Random)
        {
            m_MatchableVisual.MatchableVisualScaleTween(
                Vector3.one * m_MatchableData.ClickedMultiplier,
                m_MatchableData.ScaleUpDuration,
                m_MatchableData.ScaleUpEase,
                () =>
                {
                    SetMatchableType(GameManager.Instance.Entities.GetMatchableType((int)CurrentMatchableType.MatchableColor + 1));
                    m_MatchableVisual.MatchableVisualScaleTween(
                        Vector3.one,
                        m_MatchableData.ScaleDownDuration,
                        m_MatchableData.ScaleDownEase,
                        () =>
                        {
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
    #region SpawnTween 
    private string m_MatchableMoveTweenID;
    public Tween MoveMatchable(Vector3 _targetPos, float _duration, Ease _ease, TweenCallback _onComplete = null)
    {
        DOTween.Kill(m_MatchableMoveTweenID);
        return transform.DOMove(_targetPos, _duration)
        .OnComplete(() => { _onComplete?.Invoke(); })
        .SetEase(_ease)
        .SetId(m_MatchableMoveTweenID);
    }
    private string m_StartSpawnSequenceDelayID;
    private float m_SpawnTweenDelay;
    public void StartSpawnSequence()
    {
        m_SpawnTweenDelay = m_CurrentNode.NodeYIndis * 0.5f;
        m_SpawnTweenDelay += 0.2f;
        DOTween.Kill(m_StartSpawnSequenceDelayID);
        DOVirtual.DelayedCall(m_SpawnTweenDelay, SpawnSequence)
        .SetId(m_StartSpawnSequenceDelayID);
    }
    private string m_SpawnSequenceID;
    private Sequence m_SpawnSequence;
    public void SpawnSequence()
    {
        DOTween.Kill(m_SpawnSequenceID);
        m_SpawnSequence.Append(MoveMatchable(m_CurrentNode.GetNodePosition(), m_MatchableData.SpawnMoveDuration, m_MatchableData.SpawnMoveEase, StartGameByMatchable));
        m_SpawnSequence.Join(m_MatchableVisual.MatchableVisualScaleTween(Vector3.one, m_MatchableData.SpawnScaleDuration, m_MatchableData.SpawnScaleEase));
        m_SpawnSequence.AppendCallback(() => GameManager.Instance.Entities.CheckGrid());
    }
    #endregion

    private void StartGameByMatchable()
    {
        if (m_CurrentNode.NodeXIndis == GameManager.Instance.LevelManager.CurrentRowCount - 1 && m_CurrentNode.NodeYIndis == GameManager.Instance.LevelManager.CurrentColumnCount - 1)
        {
            GameManager.Instance.GameStart();
        }
    }

    private void CheckBlastable()
    {
        if (m_NeighbourNodes[(int)NeighbourType.Up] != null && m_NeighbourNodes[(int)NeighbourType.Down] != null)
        {
            if (m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor && m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor)
            {
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Up].MatchableOnNode);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Down].MatchableOnNode);
            }
        }
        if (m_NeighbourNodes[(int)NeighbourType.Left] != null && m_NeighbourNodes[(int)NeighbourType.Right] != null)
        {
            if (m_NeighbourNodes[(int)NeighbourType.Left].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor && m_NeighbourNodes[(int)NeighbourType.Right].MatchableOnNode.CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor)
            {
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Left].MatchableOnNode);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourNodes[(int)NeighbourType.Right].MatchableOnNode);
            }
        }
    }
    public void BlastMatchable()
    {
        GameManager.Instance.ObjectPool.SpawnFromPool(
        PooledObjectType.Blast_VFX,
        transform.position,
        Quaternion.identity,
        GameManager.Instance.Entities.GetActiveParent(ActiveParents.VFXActiveParent)
    );
        if (GameManager.Instance.PlayerManager.Player.PlayerStateMachine.CompareState(PlayerStates.RunState))
        {
            GameManager.Instance.LevelManager.DecreaseTargetMatchable(this);
        }
        OnObjectDeactive();
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_CompleteClickedDelayID);
        DOTween.Kill(m_SpawnSequenceID);
        DOTween.Kill(m_MatchableMoveTweenID);
        DOTween.Kill(m_StartSpawnSequenceDelayID);
    }

    private void OnDestroy()
    {
        KillAllTween();
        m_MatchableVisual.ResetMatchableVisual();
    }
}
