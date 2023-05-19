using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchableVisual : CustomBehaviour<Matchable>
{
    [SerializeField] private SpriteRenderer m_MatchableRenderer;
    [SerializeField] private TextMeshPro m_MatchableValueText;
    public override void Initialize(Matchable _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    public void SetMatchableVisual(MatchableType _matchableType)
    {
        SetMatchableColor(Colors.ColorArray[(int)_matchableType]);
        SetMatchableText((int)_matchableType);
    }
    private Color m_MatchableColor;
    private void SetMatchableColor(string _colorHex)
    {
        ColorUtility.TryParseHtmlString(_colorHex, out m_MatchableColor);
        m_MatchableRenderer.color = m_MatchableColor;
    }
    private void SetMatchableText(int _matchableValue)
    {
        m_MatchableValueText.text = (_matchableValue+1).ToString();
    }
}
