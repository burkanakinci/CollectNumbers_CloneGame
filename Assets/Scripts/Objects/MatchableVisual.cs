using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MatchableVisual : CustomBehaviour<Matchable>
{
    [SerializeField] private SpriteRenderer m_FrontRenderer;
    [SerializeField] private SpriteRenderer m_BackRenderer;
    [SerializeField] private TrailRenderer m_MatchableTrail;
    private Material m_MatchableMaterial;
    private Gradient m_TrailColor;
    private GradientColorKey[] m_TrailColorKey;
    private GradientAlphaKey[] m_TrailAlphaKey;
    #region Initialize
    public override void Initialize(Matchable _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_MatchableMaterial = m_FrontRenderer.material;
        InitializeTrailColor();
        InitializeTween();
    }
    private void InitializeTween()
    {
        m_VisualScaleTweenID = GetInstanceID() + "m_VisualScaleTweenID";
        m_DissolveTweenID = GetInstanceID() + "m_DissolveTweenID";
        m_PunchTweenID = GetInstanceID() + "m_PunchTweenID";
    }
    private void InitializeTrailColor()
    {
        SetTrailActive(false);
        m_TrailColor = new Gradient();
        m_TrailColorKey = new GradientColorKey[2];
        m_TrailColorKey[0].color = Color.red;
        m_TrailColorKey[0].time = 0.0f;
        m_TrailColorKey[1].color = Color.grey;
        m_TrailColorKey[1].time = 1.0f;
        m_TrailAlphaKey = new GradientAlphaKey[2];
        m_TrailAlphaKey[0].alpha = 1.0f;
        m_TrailAlphaKey[0].time = 0.0f;
        m_TrailAlphaKey[1].alpha = 0.0f;
        m_TrailAlphaKey[1].time = 1.0f;
        m_TrailColor.SetKeys(m_TrailColorKey, m_TrailAlphaKey);
    }
    #endregion
    public void SetVisual(MatchableColor _matchableColor)
    {
        m_FrontRenderer.sprite = CachedComponent.CurrentMatchableType.MatchableSprite;
        m_BackRenderer.sprite = CachedComponent.CurrentMatchableType.BackSprite;
        m_MatchableMaterial.SetFloat(Constant.DISSOLVE_VALUE, 1.0f);

        m_TrailColorKey[0].color = CachedComponent.CurrentMatchableType.TrailColor;
        m_TrailColor.SetKeys(m_TrailColorKey, m_TrailAlphaKey);
        m_MatchableTrail.colorGradient = m_TrailColor;
    }
    public void SetTrailActive(bool _isActive)
    {
        m_MatchableTrail.gameObject.SetActive(_isActive);
    }
    private string m_VisualScaleTweenID;
    public Tween ScaleTween(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_VisualScaleTweenID);
        return transform.DOScale(_target, _duration)
        .SetEase(_ease)
        .SetId(m_VisualScaleTweenID);
    }
    private string m_PunchTweenID;
    public Tween PunchTween(float _multiplier, float _duration, Ease _ease)
    {
        DOTween.Kill(m_PunchTweenID);
        return transform.DOPunchScale(
            Vector3.one * _multiplier,
            _duration,
            1,
            1.0f
        )
        .SetEase(_ease)
        .SetId(m_PunchTweenID);
    }
    private string m_DissolveTweenID;
    private float m_TempDissolveValue;
    public Tween DissolveTween(float _duration, Ease _ease)
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
        SetTrailActive(false);
        KillAllTween();
        m_MatchableMaterial.SetFloat(Constant.DISSOLVE_VALUE, 1.0f);
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_VisualScaleTweenID);
        DOTween.Kill(m_DissolveTweenID);
    }
}
