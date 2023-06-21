using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class MainMenuPanel : UIPanel
{
    [Header("Buttons")]
    [SerializeField] private StartGameButton m_StartGameButton;

    [Space(25)]
    [Header("Buttons")]
    [SerializeField] private RectMask2D m_MainMenuMask;
    public float ScreenWidth { get; private set; }
    public float ScreenHeight { get; private set; }
    [SerializeField] private StarBG[] m_BGStars;
    public override void Initialize(UIManager _uiManager)
    {
        base.Initialize(_uiManager);
        m_MaskTweenID = GetInstanceID() + "m_MaskTweenID";
        m_StartGameButton.Initialize(this);
        ScreenWidth = Screen.width / 2.0f;
        ScreenHeight = Screen.height / 2.0f;
        for (int _starCount = 0; _starCount < m_BGStars.Length; _starCount++)
        {
            m_BGStars[_starCount].Initialize(this);
        }
    }
    public override void ShowPanel()
    {
        m_MaskPaddingValue = Vector4.zero;
        SetMaskPadding(m_MaskPaddingValue);
        base.ShowPanel();
        for (int _starCount = 0; _starCount < m_BGStars.Length; _starCount++)
        {
            m_BGStars[_starCount].BGStarTween();
        }
    }
    public override void HidePanel()
    {
        KillAllTween();
        for (int _starCount = 0; _starCount < m_BGStars.Length; _starCount++)
        {
            m_BGStars[_starCount].KillAllTween();
        }
        base.HidePanel();
    }
    private float m_MaskTweenLerp;
    private string m_MaskTweenID;
    private Vector4 m_MaskPaddingValue;
    public void StartMainMenuDisolve()
    {
        DOTween.Kill(m_MaskTweenID);
        DOTween.To(() => 0.0f, x => m_MaskTweenLerp = x, 1.0f, 0.70f)
        .OnUpdate(() => { DissolveMainMenu(m_MaskTweenLerp); })
        .OnComplete(CompleteMainDissolve)
        .SetEase(Ease.Linear)
        .SetId(m_MaskTweenID);
    }
    private void CompleteMainDissolve()
    {
        HidePanel();
        GameManager.Instance.GameStart();
    }
    private void SetMaskPadding(Vector4 _padding)
    {
        m_MainMenuMask.padding = _padding;
    }
    private void DissolveMainMenu(float _lerp)
    {
        m_MaskPaddingValue.x = Mathf.Lerp(0.0f, ScreenWidth * 2.0f, _lerp);
        SetMaskPadding(m_MaskPaddingValue);
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_MaskTweenID);
    }
    private void OnDestroy()
    {
        KillAllTween();
    }
}