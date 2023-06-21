using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RunState : IPlayerState
{
    public Action OnEnterEvent { get; set; }
    public Action OnExitEvent { get; set; }
    private Player m_Player;
    public RunState(Player _player)
    {
        m_Player = _player;
    }

    public void Enter()
    {
        GameManager.Instance.LevelManager.SetLevelNumber(m_Player.PlayerData.PlayerLevel);
        GameManager.Instance.LevelManager.GetLevelData();
        GameManager.Instance.LevelManager.SetTargetMatchableList();
        GameManager.Instance.UIManager.GetPanel(UIPanelType.HudPanel).ShowPanel();
        GameManager.Instance.CameraManager.SetCamera();
        GameManager.Instance.LevelManager.CreateLevel();
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
