using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private IPlayerState m_CurrentState;
    private IPlayerState m_GeneralState;
    private List<IPlayerState> m_States;
    public PlayerStateMachine(Player _player)
    {
        m_States = new List<IPlayerState>();
        m_States.Add(new IdleState(_player));
        m_States.Add(new RunState(_player));
        m_States.Add(new WinState(_player));
        m_States.Add(new FailState(_player));
        m_States.Add(new GeneralState(_player));

        m_CurrentState = m_States[0];
        m_GeneralState = m_States[m_States.Count - 1];
    }
    public bool CompareState(PlayerStates _state)
    {
        return (m_CurrentState == m_States[(int)_state]);
    }
    public IPlayerState GetPlayerState(PlayerStates _state)
    {
        return m_States[(int)_state];
    }
    public void ChangeStateTo(PlayerStates state, bool _force = false)
    {
        if (m_CurrentState != m_States[(int)state] || _force)
        {
            Exit();
            m_CurrentState = m_States[(int)state];
            Enter();
        }
    }
    public void Enter()
    {
        m_CurrentState.Enter();
        m_GeneralState.Enter();
    }
    public void LogicalUpdate()
    {
        m_CurrentState.UpdateLogic();
        m_GeneralState.UpdateLogic();
    }
    public void PhysicalUpdate()
    {
        m_CurrentState.UpdatePhysic();
        m_GeneralState.UpdatePhysic();
    }
    public void Exit()
    {
        m_CurrentState.Exit();
        m_GeneralState.Exit();
    }
}
