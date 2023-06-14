using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MatchableVisual : CustomBehaviour<Matchable>
{
    [SerializeField] private SpriteRenderer m_FrontRenderer;
    [SerializeField] private SpriteRenderer m_BackRenderer;
    private Material m_MatchableMaterial;
    public override void Initialize(Matchable _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_MatchableMaterial = m_FrontRenderer.material;
        m_VisualScaleTweenID = GetInstanceID() + "m_VisualScaleTweenID";
        m_DissolveTweenID = GetInstanceID() + "m_DissolveTweenID";
    }
    public void SetVisual(MatchableColor _matchableColor)
    {
        m_FrontRenderer.sprite = CachedComponent.CurrentMatchableType.MatchableSprite;
        m_BackRenderer.sprite = CachedComponent.CurrentMatchableType.BackSprite;
        DOTween.Kill(m_DissolveTweenID);
        m_MatchableMaterial.SetFloat(Constant.DISSOLVE_VALUE, 1.0f);
    }
    private string m_VisualScaleTweenID;
    public Tween ScaleTween(Vector3 _target, float _duration, Ease _ease, TweenCallback _onComplete = null)
    {
        DOTween.Kill(m_VisualScaleTweenID);
        return transform.DOScale(_target, _duration)
        .OnComplete(() => { _onComplete?.Invoke(); })
        .SetEase(_ease)
        .SetId(m_VisualScaleTweenID);
    }
    private string m_DissolveTweenID;
    private float m_TempDissolveValue;
    public Tween DissolveTween(float _duration,Ease _ease)
    {
        DOTween.Kill(m_DissolveTweenID);
        m_TempDissolveValue = 1.0f;
        return DOTween.To(() => m_TempDissolveValue, x => m_TempDissolveValue = x, 0.0f, _duration)
        .OnUpdate(() =>
        {
            m_MatchableMaterial.SetFloat(Constant.DISSOLVE_VALUE, m_TempDissolveValue);
        })
        .SetEase(_ease)
        .SetId(m_DissolveTweenID);
    }
    public void ResetMatchableVisual()
    {
        KillAllTween();
        m_MatchableMaterial.SetFloat(Constant.DISSOLVE_VALUE, 1.0f);
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_VisualScaleTweenID);
        DOTween.Kill(m_DissolveTweenID);
    }
}
