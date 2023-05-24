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
    private void OnDestroy()
    {
        GameManager.Instance.LevelManager.OnChangeMoveCount -= SetRemaingMoveUI;
        DOTween.Kill(m_MoveChangeSequenceID);
    }
}
