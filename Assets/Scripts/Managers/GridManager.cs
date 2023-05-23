using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : CustomBehaviour
{
    private Node[,] m_NodesOnGrid;
    public override void Initialize()
    {
        m_NodesOnGrid = new Node[9, 9];
        SpawnGrid();
    }

    private void SpawnGrid()
    {
        for (int _row = 0; _row < m_NodesOnGrid.GetLength(0); _row++)
        {
            for (int _column = 0; _column < m_NodesOnGrid.GetLength(1); _column++)
            {
                m_NodesOnGrid[_row, _column] = new Node(_row, _column);
            }
        }
    }

    public Node GetNode(int _xIndis, int _yIndis)
    {
        if ((_xIndis < 0) || (_yIndis < 0) || (_xIndis >= GameManager.Instance.LevelManager.CurrentRowCount) || (_yIndis >= GameManager.Instance.LevelManager.CurrentColumnCount))
        {
            return null;
        }
        else
        {
            return m_NodesOnGrid[_xIndis, _yIndis];
        }
    }

    public bool NodeIsNeighbour(Node _node1, Node _node2)
    {
        return true;
    }
}
