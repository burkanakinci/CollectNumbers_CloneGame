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
        
    }
}
