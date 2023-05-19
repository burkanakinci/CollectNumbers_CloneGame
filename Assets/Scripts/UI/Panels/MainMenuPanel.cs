using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MainMenuPanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI m_LevelText;
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
    }
    public override void ShowPanel()
    {
        m_LevelText.text = "Level : " + GameManager.Instance.PlayerManager.Player.PlayerData.PlayerLevel;
        base.ShowPanel();
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }
}


