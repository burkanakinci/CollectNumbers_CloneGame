﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArea : CustomBehaviour<UIPanel>
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    public CanvasGroup CanvasGroup => m_CanvasGroup;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    public virtual void ShowArea()
    {
        CanvasGroup.Open();
    }

    public virtual void HideArea()
    {
        CanvasGroup.Close();
    }
}
