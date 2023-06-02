using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    private int m_NodeXIndis;
    private int m_NodeYIndis;
    public Matchable MatchableOnNode { get; private set; }

    public int NodeXIndis => m_NodeXIndis;
    public int NodeYIndis => m_NodeYIndis;
    public Node(int _xIndis, int _yIndis)
    {
        m_NodeXIndis = _xIndis;
        m_NodeYIndis = _yIndis;
    }

    public Vector3 GetNodePosition()
    {
        return new Vector3((NodeXIndis), (NodeYIndis), 0.0f);
    }

    public void SetMatchableOnNode(Matchable _matchable)
    {
        MatchableOnNode = _matchable;
    }
    private Matchable m_TempSpawnedMatchable;
    public void FillNode()
    {
        for (int _column = m_NodeYIndis + 1; _column < GameManager.Instance.LevelManager.CurrentColumnCount; _column++)
        {
            if (GameManager.Instance.GridManager.GetNode(NodeXIndis, _column).MatchableOnNode != null)
            {
                MatchableOnNode = GameManager.Instance.GridManager.GetNode(NodeXIndis, _column).MatchableOnNode;
                GameManager.Instance.GridManager.GetNode(NodeXIndis, _column).MatchableOnNode.SetMatchableCurrentNode(this);
                MatchableOnNode.SpawnSequence();
                GameManager.Instance.GridManager.GetNode(NodeXIndis, _column).SetMatchableOnNode(null);
                GameManager.Instance.GridManager.StartFillEmptyNodes();
                return;
            }
        }
        m_TempSpawnedMatchable = GameManager.Instance.ObjectPool.SpawnFromPool(
            PooledObjectType.Matchable,
            new Vector3(NodeXIndis, GameManager.Instance.CameraManager.CameraUpEndPos, 0.0f),
            Quaternion.identity,
            GameManager.Instance.Entities.GetActiveParent(ActiveParents.MatchableActiveParent)
        ).GetGameObject().GetComponent<Matchable>();
        MatchableOnNode = m_TempSpawnedMatchable;
        MatchableOnNode.SetMatchableCurrentNode(this);
        SetSpawnedMatchableType();
    }
    private void SetSpawnedMatchableType()
    {
        MatchableOnNode.SetMatchableType(GameManager.Instance.Entities.GetMatchableType(UnityEngine.Random.Range(0, ((int)MatchableColor.Random))));
        MatchableOnNode.CheckBlastable();
        if (MatchableOnNode.CanBlastable)
        {
            Debug.Log("asddasasdasdds");
            SetSpawnedMatchableType();
        }
        else
        {
            MatchableOnNode.SetMatchableVisual();
            MatchableOnNode.SpawnSequence();
            GameManager.Instance.GridManager.FillNodes();
        }
    }
}
