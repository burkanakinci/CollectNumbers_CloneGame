using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelArea : UIArea
{
    private Vector3 m_StartPos, m_TargetPos;
    [SerializeField] private TextMeshProUGUI m_LevelText;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_AreaTweenID = GetInstanceID() + "m_AreaTweenID";
        m_StartPos = transform.localPosition;
        m_TargetPos = m_StartPos + Vector3.right * -300.0f;
    }
    private string m_AreaTweenID;
    private void ShowTween()
    {
        transform.localPosition = m_TargetPos;
        DOTween.Kill(m_AreaTweenID);
        transform.DOLocalMove(m_StartPos, 0.65f)
        .SetEase(Ease.Linear)
        .SetId(m_AreaTweenID);
    }
    public override void ShowArea()
    {
        ShowTween();
        SetLevelText();
        base.ShowArea();
    }
    public override void HideArea()
    {
        base.HideArea();
        KillAllTween();
    }
    public void SetLevelText()
    {
        m_LevelText.text = "Level : " + GameManager.Instance.PlayerManager.Player.PlayerData.PlayerLevel;
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_AreaTweenID);
    }
    private void OnDestroy()
    {
        KillAllTween();
    }
}
