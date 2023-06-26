using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FinishArea : UIArea
{
    [SerializeField] private RectMask2D m_Mask;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_ShowDelayID = GetInstanceID() + "m_ShowDelayID";
    }
    private string m_ShowDelayID;
    public override void ShowArea()
    {
        DOTween.Kill(m_ShowDelayID);
        DOVirtual.DelayedCall(0.70f, () =>
        {
            GameManager.Instance.LevelManager.CleanSceneObject();
            base.ShowArea();
        })
        .SetId(m_ShowDelayID);
    }
    public override void HideArea()
    {
        KillAllTween();
        base.HideArea();
    }
    private void KillAllTween()
    {
        DOTween.Kill(m_ShowDelayID);
    }
}
