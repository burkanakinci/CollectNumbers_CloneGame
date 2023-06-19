using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HudPanel : UIPanel
{
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
        m_PanelAreas.ForEach(_area =>
        {
            _area.Initialize(this);
        });
    }
    public override void ShowPanel()
    {
        base.ShowPanel();
        m_PanelAreas.ForEach(_area =>
        {
            _area.ShowArea();
        });
    }
}


