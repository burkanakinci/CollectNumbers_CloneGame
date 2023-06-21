using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class TargetArea : UIArea
{
    [Header("Target Area Values")]
    [SerializeField] private TargetAreaValue[] m_TargetAreaValues;
    [SerializeField] private Target[] m_Targets;
    [Space(10)]

    [SerializeField] private RectTransform m_AreaTransform;
    [SerializeField] private Image m_TargetAreaBG;
    private Vector3 m_StartPos, m_TargetPos;
    private string m_AreaTweenID;
    private Vector2 m_TargetSize;
    private int m_OpenCount;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_AreaTweenID = GetInstanceID() + "m_AreaTweenID";
        m_StartPos = transform.localPosition;
        m_TargetPos = m_StartPos + Vector3.right * -800.0f;
        m_TargetSize.x = m_AreaTransform.sizeDelta.x;

        for (int _count = 0; _count < m_Targets.Length; _count++)
        {
            m_Targets[_count].Initialize(this);
        }
        m_OpenCount = -5;
    }
    public override void ShowArea()
    {
        SetTargetArea();
        ShowTween();
        base.ShowArea();
    }
    private void ShowTween()
    {
        transform.localPosition = m_TargetPos;
        DOTween.Kill(m_AreaTweenID);
        transform.DOLocalMove(m_StartPos, 0.65f)
        .SetEase(Ease.Linear)
        .SetId(m_AreaTweenID);
    }
    public override void HideArea()
    {
        base.HideArea();
        KillAllTween();
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_AreaTweenID);
    }

    private void SetTargetArea()
    {
        m_OpenCount = GameManager.Instance.LevelManager.CurrentTargetMatchables.Count;
        m_TargetSize.y = m_TargetAreaValues[(int)m_OpenCount - 1].BGHeight;
        m_AreaTransform.sizeDelta = m_TargetSize;
        m_TargetAreaBG.sprite = m_TargetAreaValues[(int)m_OpenCount - 1].AreaBG;
        SetTargets();
    }
    private void SetTargets()
    {
        for (int _count = 0; _count < m_Targets.Length; _count++)
        {
            if (_count < m_OpenCount)
            {
                m_Targets[_count].StartTarget(GameManager.Instance.LevelManager.CurrentTargetMatchables[_count].TargetMatchableColor,
                GameManager.Instance.LevelManager.CurrentTargetMatchables[_count].TargetMatchableCount);
            }
            else
            {
                m_Targets[_count].CloseTarget();
            }
        }
    }
}
