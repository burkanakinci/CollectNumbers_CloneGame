using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StarBG : CustomBehaviour<MainMenuPanel>
{

    public override void Initialize(MainMenuPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        transform.localPosition = new Vector3(Random.Range(-1.0f * CachedComponent.ScreenWidth , CachedComponent.ScreenWidth ), Random.Range(-1.0f * CachedComponent.ScreenHeight , CachedComponent.ScreenHeight ), 0.0f);
        transform.localScale = Vector3.up * Random.Range(0.5f, 1.0f);
        m_BGStarSequenceID = GetInstanceID() + "m_BGStarSequenceID";
    }
    private Sequence m_BGStarSequence;
    private string m_BGStarSequenceID;
    private Vector3 m_TargetPos;
    private Vector3 m_TargetScale;
    private float m_MoveDuration;
    private float m_ScaleDuration;
    public void BGStarTween()
    {
        DOTween.Kill(m_BGStarSequenceID);
        m_TargetPos = transform.localPosition;
        m_TargetPos.x = Mathf.Clamp(m_TargetPos.x + Random.Range(-50.0f, 50.0f), CachedComponent.ScreenWidth * -1.0f, CachedComponent.ScreenWidth );
        m_TargetPos.y = Mathf.Clamp(m_TargetPos.y + Random.Range(-50.0f, 50.0f), CachedComponent.ScreenHeight * -1.0f, CachedComponent.ScreenHeight );
        m_MoveDuration = Random.Range(2.5f, 3.0f);
        m_TargetScale = Vector3.one * Random.Range(0.25f, 1.0f);
        m_ScaleDuration = Random.Range(2.4f, m_MoveDuration);
        m_BGStarSequence = DOTween.Sequence().SetId(m_BGStarSequenceID);
        m_BGStarSequence.Append(transform.DOLocalMove(m_TargetPos, m_MoveDuration).SetEase(Ease.InSine));
        m_BGStarSequence.Join(transform.DOScale(m_TargetScale, m_ScaleDuration).SetEase(Ease.InSine));
        m_BGStarSequence.AppendCallback(BGStarTween);
    }
    public void KillAllTween()
    {
        DOTween.Kill(m_BGStarSequenceID);
    }
}
