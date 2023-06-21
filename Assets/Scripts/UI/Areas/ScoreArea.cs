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
        transform.localScale = Vector3.zero;

    }
    private void SetScoreText()
    {
        m_ScoreText.text = GameManager.Instance.PlayerManager.Player.PlayerData.PlayerScore.ToString();
    }
    private string m_ScaleTweenID;
    public override void ShowArea()
    {
        SetScoreText();
        ShowTween();
        base.ShowArea();
    }
    private void ShowTween()
    {
        DOTween.Kill(m_ScaleTweenID);
        transform.DOScale(Vector3.one, 0.65f)
        .SetEase(Ease.Linear)
        .SetId(m_ScaleTweenID);
    }
    public override void HideArea()
    {
        base.HideArea();
        transform.localScale = Vector3.zero;
        KillAllTween();
    }

    private void KillAllTween()
    {
        DOTween.Kill(m_ScaleTweenID);
    }
    private void OnDestroy()
    {
        KillAllTween();
    }
}
