using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : UIBaseButton<UIPanel>
{
    [SerializeField] private bool m_IsRestart;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    protected override void OnClickAction()
    {
        if (!m_IsRestart)
        {
            GameManager.Instance.PlayerManager.Player.SetPlayerLevel(GameManager.Instance.PlayerManager.Player.PlayerData.PlayerLevel + 1);
        }
        GameManager.Instance.ResetToMainMenu();
    }
}
