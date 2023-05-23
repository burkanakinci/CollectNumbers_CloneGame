using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MatchableVisual : CustomBehaviour<Matchable>
{
    [SerializeField] private SpriteRenderer m_MatchableRenderer;
    [SerializeField] private TextMeshPro m_MatchableValueText;
    public override void Initialize(Matchable _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_VisualScaleTweenID = GetInstanceID() + "m_VisualScaleTweenID";
    }
    public void SetMatchableVisual(MatchableColor _matchableColor)
    {
        SetMatchableSpriteRenderer();
        SetMatchableText();
    }
    private Color m_CurrentMatchableColor;
    private void SetMatchableSpriteRenderer()
    {
        ColorUtility.TryParseHtmlString(Colors.ColorArray[(int)CachedComponent.CurrentMatchableType.MatchableColor], out m_CurrentMatchableColor);
        m_MatchableRenderer.color = m_CurrentMatchableColor;
        m_MatchableRenderer.sprite = CachedComponent.CurrentMatchableType.MatchableSprite;
    }
    private void SetMatchableText()
    {
        m_MatchableValueText.text = CachedComponent.CurrentMatchableType.MatchableColor != MatchableColor.Random ? CachedComponent.CurrentMatchableType.MatchableValue.ToString() : "";
    }
    private string m_VisualScaleTweenID;
    public Tween MatchableVisualScaleTween(Vector3 _target, float _duration, Ease _ease, TweenCallback _onComplete = null)
    {
        DOTween.Kill(m_VisualScaleTweenID);
        return transform.DOScale(_target, _duration)
        .OnComplete(() => { _onComplete?.Invoke(); })
        .SetEase(_ease)
        .SetId(m_VisualScaleTweenID);
    }
    public void KillAllTween()
    {
        DOTween.Kill(m_VisualScaleTweenID);
    }
}
