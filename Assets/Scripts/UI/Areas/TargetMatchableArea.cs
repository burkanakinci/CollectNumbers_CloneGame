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
    }
    public void SetTargets()
    {
        for (int _count = 0; _count < m_Targets.Length; _count++)
        {
            if (_count >= GameManager.Instance.LevelManager.CurrentTargetMatcgables.Length)
            {
                m_Targets[_count].HideArea();
                return;
            }
            m_Targets[_count].ShowArea();
            m_Targets[_count].SetTarget(GameManager.Instance.LevelManager.CurrentTargetMatcgables[_count].TargetMatchableType, GameManager.Instance.LevelManager.CurrentTargetMatcgables[_count].TargetMatchableCount);
        }
    }
}
