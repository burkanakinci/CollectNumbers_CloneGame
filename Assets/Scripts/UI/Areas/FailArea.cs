using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailArea : UIArea
{
    [SerializeField] private NextLevelButton m_NextLevelButton;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        m_NextLevelButton.Initialize(CachedComponent);
    }
    public override void ShowArea()
    {
        StartShowAreaCoroutine();
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
    private IEnumerator ShowAreaCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        base.ShowArea();
    }
}
