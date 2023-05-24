using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMatchableArea : UIArea
{
    [SerializeField] private Target[] m_Targets;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        for (int _count = 0; _count < m_Targets.Length; _count++)
        {
            m_Targets[_count].Initialize(CachedComponent);
        }
        GameManager.Instance.LevelManager.OnChangeTargetMatchable += SetTargets;
    }
    public override void ShowArea()
    {
        base.ShowArea();
    }
    public override void HideArea()
    {
        for (int _count = 0; _count < m_Targets.Length; _count++)
        {
            m_Targets[_count].HideArea();
        }
        base.HideArea();
    }
    public void SetTargets(List<TargetMatchable> _targets)
    {
        for (int _count = 0; _count < _targets.Count; _count++)
        {
            m_Targets[_count].ShowArea();
            m_Targets[_count].SetTarget(_targets[_count].TargetMatchableColor, _targets[_count].TargetMatchableCount);
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.LevelManager.OnChangeTargetMatchable -= SetTargets;
    }
}
