using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
public class PlayerManager : CustomBehaviour
{
    #region Fields
    public Player Player;
    #endregion
    public override void Initialize()
    {
        Player.Initialize(this);

        GameManager.Instance.OnResetToMainMenu += OnResetToMainMenu;
        GameManager.Instance.OnGameStart += OnGameStart;
        GameManager.Instance.OnLevelSuccess += OnLevelSuccess;
        GameManager.Instance.OnLevelFailed += OnLevelFailed;
    }
    private Coroutine m_FinishCoroutine;
    private void StartFinishCoroutine(PlayerStates _state)
    {
        GameManager.Instance.InputManager.SetCanClickable(false);
        if (m_FinishCoroutine != null)
        {
            StopCoroutine(m_FinishCoroutine);
        }
        m_FinishCoroutine = StartCoroutine(FinishCoroutine(_state));
    }
    private IEnumerator FinishCoroutine(PlayerStates _state)
    {
        yield return new WaitForSecondsRealtime(0.7f);
        Player.PlayerStateMachine.ChangeStateTo(_state);
    }
    #region Events
    public void OnResetToMainMenu()
    {
        Player.PlayerStateMachine.ChangeStateTo(PlayerStates.IdleState, true);
    }
    public void OnGameStart()
    {
        Player.PlayerStateMachine.ChangeStateTo(PlayerStates.RunState);
    }
    public void OnLevelSuccess()
    {
        StartFinishCoroutine(PlayerStates.SuccessState);
    }
    public void OnLevelFailed()
    {
        StartFinishCoroutine(PlayerStates.FailState);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnResetToMainMenu -= OnResetToMainMenu;
        GameManager.Instance.OnGameStart -= OnGameStart;
        GameManager.Instance.OnLevelSuccess -= OnLevelSuccess;
        GameManager.Instance.OnLevelFailed -= OnLevelFailed;
    }
    #endregion
}
