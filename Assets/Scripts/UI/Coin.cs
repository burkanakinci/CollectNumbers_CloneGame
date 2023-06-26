using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Coin : PooledObject
{
    [SerializeField] private CoinData m_CoinData;
    public override void Initialize()
    {
        m_ScaleTween = GetInstanceID() + "m_ScaleTween";
        m_MoveTween = GetInstanceID() + "m_MoveTween";
        m_PunchTweenID = GetInstanceID() + "m_PunchTweenID";
        ResetCoin();
    }
    public override void OnObjectSpawn()
    {
        base.OnObjectSpawn();
        ScaleTween(m_CoinData.SpawnPunchScale, m_CoinData.SpawnPunchDuration, m_CoinData.SpawnPunchEase)
        .OnComplete(() =>
        {
            ScaleTween(Vector3.one, m_CoinData.SpawnPuncDownDuration, m_CoinData.SpawnPunchDownEase)
                            .OnComplete(() =>
                            {
                                MoveTween(GameManager.Instance.Entities.CoinScorePos, m_CoinData.MoveDuration, m_CoinData.MoveEase)
                                    .OnComplete(() =>
                                    {
                                        ScaleTween(Vector3.zero, m_CoinData.SpawnPuncDownDuration, m_CoinData.SpawnPunchDownEase)
                                        .OnComplete(() =>
                                        {
                                            GameManager.Instance.PlayerManager.Player.IncreasePlayerScore(1);
                                            GameManager.Instance.GridManager.FillNodes();
                                            OnObjectDeactive();
                                        });
                                    });
                            });
        });
    }
    public override void OnObjectDeactive()
    {
        ResetCoin();
        base.OnObjectDeactive();
    }
    private string m_PunchTweenID;
    private Tween PunchCoin(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_PunchTweenID);
        return transform.DOPunchScale(_target, _duration, 1)
        .SetEase(_ease)
        .SetId(m_PunchTweenID);
    }
    private string m_ScaleTween;
    private Tween ScaleTween(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_ScaleTween);
        return transform.DOScale(_target, _duration)
        .SetEase(_ease)
        .SetId(m_ScaleTween);
    }
    private string m_MoveTween;
    private Tween MoveTween(Vector3 _target, float _duration, Ease _ease)
    {
        DOTween.Kill(m_MoveTween);
        return transform.DOMove(_target, _duration)
        .SetEase(_ease)
        .SetId(m_MoveTween);
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_PunchTweenID);
        DOTween.Kill(m_ScaleTween);
        DOTween.Kill(m_MoveTween);
    }
    private void ResetCoin()
    {
        transform.localScale = Vector3.zero;
        KillAllTween();
    }
}
