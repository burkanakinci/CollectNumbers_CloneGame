using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MoveArea : UIArea
{
    [SerializeField] private TextMeshProUGUI m_RemainingMoveText;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        GameManager.Instance.LevelManager.OnChangeMoveCount += SetRemaingMoveUI;
        m_MoveChangeSequenceID = GetInstanceID() + "m_MoveChangeTweenID";
        m_ScaleTweenID = GetInstanceID() + "m_ScaleTweenID";
        transform.localScale = Vector3.zero;
    }
    private string m_ScaleTweenID;
    public override void ShowArea()
    {
        transform.DOScale(Vector3.one, 0.75f)
        .SetEase(Ease.OutExpo)
        .SetId(m_ScaleTweenID);
        base.ShowArea();
    }
    public override void HideArea()
    {
        base.HideArea();
        transform.localScale = Vector3.zero;
        KillAllTween();
    }
    public void SetRemaingMoveUI(int _move)
    {
        ChangeMoveCountTween(_move);
    }
    private string m_MoveChangeSequenceID;
    private Sequence m_MoveChangeSequence;
    private void ChangeMoveCountTween(int _move)
    {
        DOTween.Kill(m_MoveChangeSequenceID);
        m_MoveChangeSequence = DOTween.Sequence().SetId(m_MoveChangeSequenceID);
        m_MoveChangeSequence.Append(m_RemainingMoveText.transform.DOScale(Vector3.one * 1.15f, 0.4f).SetEase(Ease.Linear));
        m_MoveChangeSequence.AppendCallback(() =>
        {
            m_RemainingMoveText.text = _move.ToString();
        });
        m_MoveChangeSequence.Append(m_RemainingMoveText.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear));
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_MoveChangeSequenceID);
        DOTween.Kill(m_ScaleTweenID);
    }
    private void OnDestroy()
    {
        GameManager.Instance.LevelManager.OnChangeMoveCount -= SetRemaingMoveUI;
        KillAllTween();
    }
}
