using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchableVisual : CustomBehaviour<Matchable>
{
    [SerializeField] private SpriteRenderer m_MatchableRenderer;
    public override void Initialize(Matchable _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    private Color m_MatchableColor;
    public void SetMatchableColor(string _colorHex)
    {
        ColorUtility.TryParseHtmlString(_colorHex, out m_MatchableColor);
        m_MatchableRenderer.color = m_MatchableColor;
    }
}
