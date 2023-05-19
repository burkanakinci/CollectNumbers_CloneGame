using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Star : CustomBehaviour<FinishArea>
{
    [SerializeField] private RectTransform m_StarRectTransform;
    public override void Initialize(FinishArea _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_StarSpawnSequenceID = GetInstanceID() + "m_StarSpawnSequenceID";
    }
    private string m_StarSpawnSequenceID;
    private Sequence m_StarSpawnSequence;
    public void StarSpawnSequence(Vector2 _targetPos)
    {
        this.gameObject.SetActive(true);
        DOTween.Kill(m_StarSpawnSequenceID);
        m_StarSpawnSequence = DOTween.Sequence().SetId(m_StarSpawnSequenceID);
        m_StarSpawnSequence.Append(m_StarRectTransform.DOScale(Vector2.one, 0.75f).SetEase(Ease.InExpo));
        m_StarSpawnSequence.Insert(0.5f, m_StarRectTransform.DOMove(_targetPos, 0.75f));
        m_StarSpawnSequence.Append(m_StarRectTransform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutExpo));
        m_StarSpawnSequence.AppendCallback(() =>
        {
            this.gameObject.SetActive(false);
        });
    }
    public void ResetStar(Vector2 _startPos)
    {
        DOTween.Kill(m_StarSpawnSequenceID);
        m_StarRectTransform.localScale = Vector2.zero;
        m_StarRectTransform.position = _startPos;
    }
}
