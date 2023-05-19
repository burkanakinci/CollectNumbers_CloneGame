using UnityEngine;

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
        Player.PlayerStateMachine.ChangeStateTo(PlayerStates.SuccessState);
    }
    public void OnLevelFailed()
    {
        Player.PlayerStateMachine.ChangeStateTo(PlayerStates.FailState);
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
