using UnityEngine;
using System.Collections.Generic;
using System;

public class Player : CustomBehaviour<PlayerManager>
{
    public PlayerData PlayerData;
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public event Action OnIncreaseScore;
    public override void Initialize(PlayerManager _playerManager)
    {
        GameManager.Instance.JsonConverter.LoadPlayerData(ref PlayerData);
        PlayerStateMachine = new PlayerStateMachine(this);
    }
    public void SetPlayerLevel(int _level)
    {
        PlayerData.PlayerLevel = _level;
        GameManager.Instance.JsonConverter.SavePlayerData(PlayerData);
    }
    public void IncreasePlayerScore(int _score)
    {
        PlayerData.PlayerScore += _score;
        GameManager.Instance.JsonConverter.SavePlayerData(PlayerData);
        OnIncreaseScore?.Invoke();
    }
    private void Update()
    {
        PlayerStateMachine.LogicalUpdate();
    }
}
