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
        m_ColorChangeTweenID = GetInstanceID() + "m_ColorChangeTweenID";
        m_TextRotateSequenceID = GetInstanceID() + "m_TextRotateTweenID";
    }
    public void SetMatchableVisual(MatchableColor _matchableColor)
    {
        SetMatchableSpriteRenderer();
        SetMatchableText();
    }
    private Color m_CurrentMatchableColor;
    private Color m_PrevColor;
    private float m_ColorChangeLerpValue;
    private string m_ColorChangeTweenID;
    private void SetMatchableSpriteRenderer()
    {
        m_CurrentMatchableColor = Colors.GetColor(CachedComponent.CurrentMatchableType.MatchableColor);
        m_PrevColor = m_MatchableRenderer.color;
        m_MatchableRenderer.sprite = CachedComponent.CurrentMatchableType.MatchableSprite;
        SetColorTween();
    }
    private void SetColorTween()
    {
        DOTween.Kill(m_ColorChangeTweenID);
        m_ColorChangeLerpValue = 0.0f;
        DOTween.To(() => m_ColorChangeLerpValue, x => m_ColorChangeLerpValue = x, 1.0f, 0.5f)
        .OnUpdate(() =>
        {
            SetColor(m_ColorChangeLerpValue);
        })
        .SetEase(Ease.Linear)
        .SetId(m_ColorChangeTweenID);
    }
    private void SetColor(float _lerp)
    {
        m_MatchableRenderer.color = Color.Lerp(m_PrevColor, m_CurrentMatchableColor, _lerp);
    }
    private Sequence m_TextRotateSequence;
    private string m_TextRotateSequenceID;
    private float m_TextRotateLerpValue;
    private void SetMatchableText()
    {
        RotateTextTween();
    }
    private void RotateTextTween()
    {
        DOTween.Kill(m_TextRotateSequenceID);
        m_TextRotateSequence = DOTween.Sequence().SetId(m_TextRotateSequenceID);
        m_TextRotateSequence.Append(m_MatchableValueText.transform.DORotateQuaternion(new Quaternion(0.0f, 0.0f, -1.0f, 0.0f), 0.15f).SetEase(Ease.Linear));
        m_TextRotateSequence.Join(m_MatchableValueText.transform.DOScale(Vector3.one * 0.75f, 0.25f));
        m_TextRotateSequence.AppendCallback(() =>
        {
            m_MatchableValueText.text = CachedComponent.CurrentMatchableType.MatchableColor != MatchableColor.Random ? CachedComponent.CurrentMatchableType.MatchableValue.ToString() : "";
        });
        m_TextRotateSequence.Append(m_MatchableValueText.transform.DORotateQuaternion(new Quaternion(0.0f, 0.0f, 0.0f, -1.0f), 0.15f).SetEase(Ease.Linear));
        m_TextRotateSequence.Join(m_MatchableValueText.transform.DOScale(Vector3.one, 0.25f));
        m_TextRotateSequence.AppendCallback(() =>
        {
            m_MatchableValueText.transform.rotation = Quaternion.identity;
        });
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
    public void ResetMatchableVisual()
    {
        KillAllTween();
        m_MatchableValueText.text = "";
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_TextRotateSequenceID);
        DOTween.Kill(m_ColorChangeTweenID);
        DOTween.Kill(m_VisualScaleTweenID);
    }
}
