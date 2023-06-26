using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class Target : CustomBehaviour<TargetArea>
{
    [SerializeField] private CanvasGroup m_TargetCanvasGroup;
    [SerializeField] private Image m_TargetIcon;
    [SerializeField] private TextMeshProUGUI m_TargetText;
    [SerializeField] private Transform m_BlastMatchablePos;
    [SerializeField] private Image m_TargetSlider;
    [SerializeField] private Transform m_CheckParent;
    [SerializeField] private Transform m_CheckVFXPos;
    private int m_CurrentTargetCount, m_StartTargetCount;
    private MatchableColor m_CurrentMatchableColor;
    private List<Matchable> m_Matched;

    #region  ExternalAccess
    public Vector3 BlastMatchablePos => m_BlastMatchablePos.position;
    #endregion
    public override void Initialize(TargetArea _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_TextTargetScaleTweenID = GetInstanceID() + "m_TextTargetScaleTweenID";
        m_TargetSliderTweenID = GetInstanceID() + "m_TargetSliderTweenID";
        m_DecreaseCompleteDelayID = GetInstanceID() + "m_DecreaseCompleteDelayID";
        m_CheckTweenID = GetInstanceID() + "m_CheckTweenID";
        m_Matched = new List<Matchable>();
        CloseTarget();
    }
    public void CloseTarget()
    {
        m_Matched.Clear();
        RemoveOnBlastFunctions();
        KillAllTween();
        m_TargetText.transform.localScale = Vector3.one;
        m_CheckParent.localScale = Vector3.zero;
        m_TargetCanvasGroup.Close();
    }
    public void StartTarget(MatchableColor _targetColor, int _targetCount)
    {
        GameManager.Instance.Entities.OnBlastMatchables += OnBlastMatchables;
        GameManager.Instance.Entities.OnAddedBlastMatchable += OnAddedBlastMatchable;
        m_TargetCanvasGroup.Open();
        m_CurrentMatchableColor = _targetColor;
        m_TargetIcon.sprite = GameManager.Instance.Entities.GetMatchableType((int)m_CurrentMatchableColor).MatchableSprite;
        m_StartTargetCount = _targetCount;
        SetTargetText(m_StartTargetCount);
        m_CurrentSliderValue = 0.0f;
        m_PreviousSliderValue = 0.0f;
        SetSlider(m_PreviousSliderValue);
    }
    private void SetTargetText(int _targetCount)
    {

        m_CurrentTargetCount = _targetCount;
        if (_targetCount > 0)
        {
            m_TargetText.text = m_CurrentTargetCount.ToString();
        }
        else
        {
            m_TargetText.text = "";
        }
    }
    private void OnAddedBlastMatchable(Matchable _added)
    {
        if (_added.CurrentMatchableType.MatchableColor == m_CurrentMatchableColor)
        {
            m_Matched.Add(_added);
            GameManager.Instance.Entities.OnBlastMatchables -= _added.BlastMatchable;
        }
    }
    private Vector3 m_CoinSpawnPos;
    public void OnBlastMatchables()
    {
        for (int _count = 0; _count < m_Matched.Count; _count++)
        {
            m_Matched[_count].TargetSequence(this);
        }
        m_Matched.Clear();
    }
    private float m_TargetDiff;
    public void DecreaseTarget()
    {
        m_CurrentTargetCount--;
        SetTargetText(m_CurrentTargetCount);
        m_PreviousSliderValue = m_CurrentSliderValue;
        m_TargetDiff = m_StartTargetCount - m_CurrentTargetCount;
        m_CurrentSliderValue = m_TargetDiff / m_StartTargetCount;
        TargetSliderTween(m_PreviousSliderValue, m_CurrentSliderValue, 0.2f);
        DecreaseTargetSequence();
        StartDecreaseComplete();
        m_CoinSpawnPos = BlastMatchablePos;
        m_CoinSpawnPos.x += Random.Range(-0.150f, 0.150f);
        m_CoinSpawnPos.y += Random.Range(-0.150f, 0.150f);
        GameManager.Instance.ObjectPool.SpawnFromPool(
            PooledObjectType.Coin,
            m_CoinSpawnPos,
            Quaternion.identity,
            GameManager.Instance.Entities.GetActiveParent(ActiveParents.UIObjects)
        );
    }
    private string m_DecreaseCompleteDelayID;
    private void StartDecreaseComplete()
    {
        DOTween.Kill(m_DecreaseCompleteDelayID);
        DOVirtual.DelayedCall(0.2f, () =>
        {
            if (m_CurrentTargetCount <= 0)
            {
                GameManager.Instance.LevelManager.DecreaseTarget(m_CurrentMatchableColor);
            }
        });
    }
    public void DecreaseTargetSequence()
    {
        TargetTextScaleTween(Vector3.one * 1.25f, 0.5f).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            SetTargetText(m_CurrentTargetCount);
            TargetTextScaleTween(Vector3.one, 0.35f).SetEase(Ease.Linear);
        });
    }
    private float m_CurrentSliderValue, m_PreviousSliderValue;
    private Vector3 m_TempCheckVFXPos;
    private void SetSlider(float _amount)
    {
        m_TargetSlider.fillAmount = _amount;
        if (_amount >= 1.0f)
        {
            m_TempCheckVFXPos = m_CheckVFXPos.position;
            m_TempCheckVFXPos.z = 0.0f;
            GameManager.Instance.ObjectPool.SpawnFromPool(
                PooledObjectType.Star_VFX,
                m_TempCheckVFXPos,
                Quaternion.identity,
                GameManager.Instance.Entities.GetActiveParent(ActiveParents.VFXActiveParent)
            );
            OpenTargetCheck();
        }
    }
    private string m_CheckTweenID;
    private void OpenTargetCheck()
    {
        DOTween.Kill(m_CheckTweenID);
        m_CheckParent.DOScale(Vector3.one * 1.15f, 0.35f).SetId(m_CheckTweenID)
        .OnComplete(() =>
        {
            m_CheckParent.DOScale(Vector3.one, 0.15f).SetId(m_CheckTweenID);
        });
    }
    private string m_TextTargetScaleTweenID;
    private Tween TargetTextScaleTween(Vector3 _target, float _duration)
    {
        DOTween.Kill(m_TextTargetScaleTweenID);
        return m_TargetText.transform.DOScale(_target, _duration);
    }
    private string m_TargetSliderTweenID;
    private Tween TargetSliderTween(float _start, float _end, float _duration)
    {
        DOTween.Kill(m_TargetSliderTweenID);
        return DOTween.To(() => _start, x => SetSlider(x), _end, _duration);
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_DecreaseCompleteDelayID);
        DOTween.Kill(m_TextTargetScaleTweenID);
        DOTween.Kill(m_TargetSliderTweenID);
    }
    private void OnDestroy()
    {
        RemoveOnBlastFunctions();
    }
    private void RemoveOnBlastFunctions()
    {
        GameManager.Instance.Entities.OnBlastMatchables -= OnBlastMatchables;
        GameManager.Instance.Entities.OnAddedBlastMatchable -= OnAddedBlastMatchable;
    }
}
