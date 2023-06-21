using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Target : CustomBehaviour<TargetArea>
{
    [SerializeField] private CanvasGroup m_TargetCanvasGroup;
    [SerializeField] private Image m_TargetIcon;
    [SerializeField] private TextMeshProUGUI m_TargetText;
    private int m_CurrentTargetCount;
    private MatchableColor m_CurrentMatchableColor;
    private List<Matchable> m_Matched;
    public override void Initialize(TargetArea _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_Matched = new List<Matchable>();
    }
    public void CloseTarget()
    {
        GameManager.Instance.Entities.OnAddedBlastMatchable -= OnAddedBlastMatchable;
        m_TargetCanvasGroup.Close();
    }
    public void StartTarget(MatchableColor _targetColor, int _targetCount)
    {
        GameManager.Instance.Entities.OnAddedBlastMatchable += OnAddedBlastMatchable;
        m_TargetCanvasGroup.Open();
        m_CurrentMatchableColor = _targetColor;
        m_TargetIcon.sprite = GameManager.Instance.Entities.GetMatchableType((int)m_CurrentMatchableColor).MatchableSprite;
        SetTargetText(_targetCount);
    }
    private void SetTargetText(int _targetCount)
    {
        m_CurrentTargetCount = _targetCount;
        m_TargetText.text = m_CurrentTargetCount.ToString();
    }
    private void OnAddedBlastMatchable(Matchable _added)
    {
        if (_added.CurrentMatchableType.MatchableColor == m_CurrentMatchableColor)
        {
            m_Matched.Add(_added);
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.Entities.OnAddedBlastMatchable -= OnAddedBlastMatchable;
    }
}
