using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Target : UIArea
{
    [SerializeField] private TextMeshProUGUI m_TargetText;
    [SerializeField] private Image m_TargetIcon;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        transform.localScale = Vector3.zero;
        m_TargetScaleUpTweenID = GetInstanceID() + "m_TargetScaleUpTweenID";
    }
    public override void HideArea()
    {
        base.HideArea();
        TargetScaleUpTween(Vector3.zero);
    }
    public override void ShowArea()
    {
        base.ShowArea();
        TargetScaleUpTween(Vector3.one);
    }
    private string m_TargetScaleUpTweenID;
    private void TargetScaleUpTween(Vector3 _target)
    {
        DOTween.Kill(m_TargetScaleUpTweenID);
        transform.DOScale(_target, 0.5f)
        .SetEase(Ease.OutExpo)
        .SetId(m_TargetScaleUpTweenID);
    }
    private Color m_TargetCurrentColor;
    public void SetTarget(MatchableColor _color, int _count)
    {
        m_TargetCurrentColor = Colors.GetColor(_color);
        m_TargetIcon.color = m_TargetCurrentColor;
        m_TargetText.text = _count > 0 ? _count.ToString() : "";
    }
}
