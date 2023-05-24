using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HudPanel : UIPanel
{
    [SerializeField] private TextMeshProUGUI m_LevelText;
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
    }
    public override void ShowPanel()
    {
        base.ShowPanel();
        m_LevelText.text = "Level : " + GameManager.Instance.PlayerManager.Player.PlayerData.PlayerLevel;
        m_PanelAreas.ForEach(_area =>
        {
            _area.ShowArea();
        });
    }
}


