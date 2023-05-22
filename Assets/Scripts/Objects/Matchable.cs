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
    private Matchable[] m_NeighbourMatchables;
    public override void Initialize()
    {
        m_MatchableVisual.Initialize(this);
        m_NeighbourMatchables = new Matchable[4];
        m_SpawnSequenceID = GetInstanceID() + "m_SpawnSequenceID";
        m_MatchableMoveTweenID = GetInstanceID() + "m_MatchableMoveTweenID";
        m_StartSpawnSequenceDelayID = GetInstanceID() + "m_StartSpawnSequenceDelayID";
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        m_MatchableVisual.transform.localScale = Vector3.zero;
        GameManager.Instance.Entities.OnCheckBlast += CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn+=StartSpawnSequence;
    }
    public override void OnObjectDeactive()
    {
        KillAllTween();
        m_MatchableVisual.KillAllTween();
        GameManager.Instance.Entities.OnCheckBlast -= CheckBlastable;
        GameManager.Instance.Entities.OnCompleteSpawn+=StartSpawnSequence;
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
        m_CurrentNode.MatchableOnNode = this;
    }
    private void SetNeighbours()
    {
        m_NeighbourMatchables[(int)NeighbourType.Up] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis + 1).MatchableOnNode;
        m_NeighbourMatchables[(int)NeighbourType.Down] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis, m_CurrentNode.NodeYIndis - 1).MatchableOnNode;
        m_NeighbourMatchables[(int)NeighbourType.Left] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis - 1, m_CurrentNode.NodeYIndis).MatchableOnNode;
        m_NeighbourMatchables[(int)NeighbourType.Right] = GameManager.Instance.GridManager.GetNode(m_CurrentNode.NodeXIndis + 1, m_CurrentNode.NodeYIndis).MatchableOnNode;
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
    #region SpawnTween 
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
        m_SpawnTweenDelay = m_CurrentNode.NodeYIndis * 0.5f;
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
    #endregion

    private void CheckBlastable()
    {
        if (m_NeighbourMatchables[(int)NeighbourType.Up] != null && m_NeighbourMatchables[(int)NeighbourType.Down] != null)
        {
            if (m_NeighbourMatchables[(int)NeighbourType.Up].CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor && m_NeighbourMatchables[(int)NeighbourType.Down].CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor)
            {
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourMatchables[(int)NeighbourType.Up]);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourMatchables[(int)NeighbourType.Down]);
            }
        }
        if (m_NeighbourMatchables[(int)NeighbourType.Left] != null && m_NeighbourMatchables[(int)NeighbourType.Right] != null)
        {
            if (m_NeighbourMatchables[(int)NeighbourType.Left].CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor && m_NeighbourMatchables[(int)NeighbourType.Right].CurrentMatchableType.MatchableColor == CurrentMatchableType.MatchableColor)
            {
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, this);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourMatchables[(int)NeighbourType.Left]);
                GameManager.Instance.Entities.SetBlastedMatchables(ListOperations.Adding, m_NeighbourMatchables[(int)NeighbourType.Right]);
            }
        }
    }
    public void BlastMatchable()
    {
        
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_SpawnSequenceID);
        DOTween.Kill(m_MatchableMoveTweenID);
        DOTween.Kill(m_StartSpawnSequenceDelayID);
    }
}
