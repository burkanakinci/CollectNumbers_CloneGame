using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FailArea : UIArea
{
    public override void Initialize(UIPanel _cachedComponent)
    {
        m_ShowDelayID = GetInstanceID() + "m_ShowDelayID";
        base.Initialize(_cachedComponent);
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
