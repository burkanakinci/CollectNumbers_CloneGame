using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreArea : UIArea
{
    [SerializeField] private TextMeshProUGUI m_ScoreText;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_ScaleTweenID = GetInstanceID() + "m_ScaleTweenID";
        m_TextScaleTweenID = GetInstanceID() + "m_TextScaleTweenID";
        transform.localScale = Vector3.zero;

    }
    private void SetScoreText()
    {
        m_ScoreText.text = GameManager.Instance.PlayerManager.Player.PlayerData.PlayerScore.ToString();
    }

    public override void ShowArea()
    {
        GameManager.Instance.PlayerManager.Player.OnIncreaseScore += OnIncreaseScore;
        SetScoreText();
        ScaleTween(Vector3.one, 0.65f, Ease.Linear);
        base.ShowArea();
    }
    private string m_ScaleTweenID;
    private Tween ScaleTween(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_ScaleTweenID);
        return transform.DOScale(_target, _duration)
        .SetEase(_ease)
        .SetId(m_ScaleTweenID);
    }
    private string m_TextScaleTweenID;
    private Tween TextScaleTween(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_TextScaleTweenID);
        return m_ScoreText.transform.DOScale(_target, _duration)
        .SetEase(_ease)
        .SetId(m_ScaleTweenID);
    }
    public override void HideArea()
    {
        GameManager.Instance.PlayerManager.Player.OnIncreaseScore -= OnIncreaseScore;
        transform.localScale = Vector3.zero;
        KillAllTween();
        base.HideArea();
    }
    private void OnIncreaseScore()
    {
        TextScaleTween(Vector3.one * 1.2f, 0.4f, Ease.Linear)
        .OnComplete(() =>
        {
            SetScoreText();
            TextScaleTween(Vector3.one, 0.2f, Ease.Linear);
        });
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_TextScaleTweenID);
        DOTween.Kill(m_ScaleTweenID);
    }
    private void OnDestroy()
    {
        KillAllTween();
        GameManager.Instance.PlayerManager.Player.OnIncreaseScore -= OnIncreaseScore;
    }
}
