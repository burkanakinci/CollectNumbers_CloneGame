using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FinishArea : UIArea
{
    [SerializeField] private TextMeshProUGUI m_PlayerScoreText;
    [SerializeField] private TextMeshProUGUI m_LevelEarnedText;
    [SerializeField] private RectTransform m_NextLevelButtonTransform;
    [SerializeField] private NextLevelButton m_NextLevelButton;
    [SerializeField] private List<Star> m_SuccessStars;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_NextLevelButton.Initialize(CachedComponent);
        m_NextLevelButtonScaleTweenID = GetInstanceID() + "m_NextLevelButtonScaleTweenID";
        m_SuccessStars.ForEach(_star =>
        {
            _star.Initialize(this);
        });
    }
    public override void ShowArea()
    {
        StartShowAreaCoroutine();
    }
    public override void HideArea()
    {
        m_NextLevelButtonTransform.localScale = Vector3.zero;
        DOTween.Kill(m_NextLevelButtonScaleTweenID);
        m_SuccessStars.ForEach(_star =>
        {
            _star.ResetStar(m_LevelEarnedText.rectTransform.position);
        });
        base.HideArea();
    }
    private Coroutine m_ShowAreaCoroutine;
    private void StartShowAreaCoroutine()
    {
        if (m_ShowAreaCoroutine != null)
        {
            StopCoroutine(m_ShowAreaCoroutine);
        }
        m_ShowAreaCoroutine = StartCoroutine(ShowAreaCoroutine());
    }
    private int m_EarnedOnLevel;
    private IEnumerator ShowAreaCoroutine()
    {
        yield return new WaitForSeconds(3.5f);
        m_TempEarnedValue = 0;
        SetLevelEarnedText();
        SetPlayerScoreText();
        m_EarnedOnLevel = 10;
        m_NextLevelButtonTransform.localScale = Vector3.zero;
        m_SuccessStars.ForEach(_star =>
        {
            _star.ResetStar(m_LevelEarnedText.rectTransform.position);
        });
        StartLevelEarnedCoroutine();
        base.ShowArea();
    }
    private int m_TempEarnedValue;
    private void SetPlayerScoreText()
    {
        m_PlayerScoreText.text = GameManager.Instance.PlayerManager.Player.PlayerData.PlayerScore.ToString();
    }
    private void SetLevelEarnedText()
    {
        m_LevelEarnedText.text = m_TempEarnedValue > 0 ? "x" + m_TempEarnedValue.ToString() : "";
    }
    private Coroutine m_IncreaseScoreCoroutine;
    private void StartLevelEarnedCoroutine()
    {
        if (m_IncreaseScoreCoroutine != null)
        {
            StopCoroutine(m_IncreaseScoreCoroutine);
        }
        m_IncreaseScoreCoroutine = StartCoroutine(LevelEarnedCoroutine());
    }
    private IEnumerator LevelEarnedCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        if (m_EarnedOnLevel > 0)
        {
            m_EarnedOnLevel--;
            m_TempEarnedValue++;

            GameManager.Instance.PlayerManager.Player.IncreasePlayerScore(1);
            SetPlayerScoreText();
            SetLevelEarnedText();
            m_SuccessStars[m_EarnedOnLevel].StarSpawnSequence(m_PlayerScoreText.rectTransform.position);
            StartLevelEarnedCoroutine();
        }
        else
        {
            NextLevelButtonScaleTween();
        }
    }

    private string m_NextLevelButtonScaleTweenID;
    private void NextLevelButtonScaleTween()
    {
        DOTween.Kill(m_NextLevelButtonScaleTweenID);
        m_NextLevelButtonTransform.DOScale(Vector3.one, 1.5f)
        .SetEase(Ease.OutExpo)
        .SetId(m_NextLevelButtonScaleTweenID);
    }
}
