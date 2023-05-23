using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HudPanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI m_LevelText;
    [SerializeField] private TargetMatchableArea m_TargetMatchableArea;
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
        m_TargetMatchableArea.Initialize(this);
    }
    public override void ShowPanel()
    {
        m_LevelText.text = "Level : " + GameManager.Instance.PlayerManager.Player.PlayerData.PlayerLevel;
        m_TargetMatchableArea.SetTargets();
        base.ShowPanel();
    }
}


