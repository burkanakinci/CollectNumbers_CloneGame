using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class IdleState : IPlayerState
{
    public Action OnEnterEvent { get; set; }
    public Action OnExitEvent { get; set; }
    private Player m_Player;
    public IdleState(Player _player)
    {
        m_Player = _player;
    }

    public void Enter()
    {
        GameManager.Instance.UIManager.GetPanel(UIPanelType.MainMenuPanel).ShowPanel();
        OnEnterEvent?.Invoke();
    }
    public void UpdateLogic()
    {

    }
    public void UpdatePhysic()
    {

    }
    public void Exit()
    {
    }
    public void TriggerEnter(Collider _other)
    {
    }
}
