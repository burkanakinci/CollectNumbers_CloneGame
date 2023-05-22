using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    private int m_NodeXIndis;
    private int m_NodeYIndis;
    public Matchable MatchableOnNode;

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
}
