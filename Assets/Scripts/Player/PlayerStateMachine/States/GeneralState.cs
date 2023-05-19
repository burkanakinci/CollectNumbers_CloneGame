using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GeneralState : IPlayerState
{
    public Action OnEnterEvent { get; set; }
    public Action OnExitEvent { get; set; }
    private Player m_Player;
    public GeneralState(Player _player)
    {
        m_Player = _player;
    }

    public void Enter()
    {
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
        OnExitEvent?.Invoke();
    }
    public void TriggerEnter(Collider _other)
    {
    }
}
