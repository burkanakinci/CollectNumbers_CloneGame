using UnityEngine;
using System.Collections.Generic;
using System;

public class UIPanel : CustomBehaviour<UIManager>
{
    [SerializeField] private CanvasGroup m_CanvasGroup;
    public CanvasGroup CanvasGroup => m_CanvasGroup;
    [SerializeField] protected List<UIArea> m_PanelAreas;
    public UIArea CurrentArea { get; private set; }
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
        m_PanelAreas.ForEach(_area =>
        {
            _area.gameObject.SetActive(true);
            _area.Initialize(this);
        });
    }

    public virtual void ShowPanel()
    {
        CachedComponent.HideAllPanels();

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        CanvasGroup.Open();
        SetCurrentPanel();
    }
    public virtual void ShowPanel(Action _complete)
    {
        CachedComponent.HideAllPanels();

        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        CanvasGroup.Open();
        SetCurrentPanel();
    }

    public virtual void HidePanel()
    {
        CanvasGroup.Close();
    }

    public virtual void SetCurrentPanel()
    {
        CachedComponent.SetCurrentUIPanel(this);
    }
    public virtual void HideAllArea()
    {
        m_PanelAreas.ForEach(_area =>
        {
            _area.HideArea();
        });
    }
    public virtual void ShowArea<T>(T _area) where T : Enum
    {
        CurrentArea = m_PanelAreas[(int)(object)_area];
        CurrentArea.ShowArea();

    }
}
