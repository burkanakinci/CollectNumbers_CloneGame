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
    private Coroutine m_FillNodesCoroutine;
    public void StartFillEmptyNodes()
    {
        if (m_FillNodesCoroutine != null)
        {
            StopCoroutine(m_FillNodesCoroutine);
        }
        m_FillNodesCoroutine = StartCoroutine(FillNodesCoroutine());
    }
    private IEnumerator FillNodesCoroutine()
    {
        yield return new WaitForEndOfFrame();
        FillNodes();
    }
    public void FillNodes()
    {
        for (int _row = 0; _row < GameManager.Instance.LevelManager.CurrentRowCount; _row++)
        {
            for (int _column = 0; _column < GameManager.Instance.LevelManager.CurrentColumnCount; _column++)
            {
                if (GetNode(_row, _column).MatchableOnNode == null)
                {
                    if (GameManager.Instance.PlayerManager.Player.PlayerStateMachine.CompareState(PlayerStates.RunState))
                    {
                        GetNode(_row, _column).FillNode();
                        return;
                    }
                }
            }
        }
        StartCheckGridCoroutine();
    }
    private Coroutine m_CheckGridCoroutine;
    private void StartCheckGridCoroutine()
    {
        if (m_CheckGridCoroutine != null)
        {
            StopCoroutine(m_CheckGridCoroutine);
        }
        m_CheckGridCoroutine = StartCoroutine(CheckGridCoroutine());
    }
    private IEnumerator CheckGridCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.Entities.CheckGrid();
    }
}
