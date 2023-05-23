using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Target : UIArea
{
    [SerializeField] private TextMeshProUGUI m_TargetText;
    [SerializeField] private Image m_TargetIcon;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
    }
    private Color m_TargetCurrentColor;
    public void SetTarget(MatchableColor _color, int _count)
    {
        m_TargetText.text = _count.ToString();
        ColorUtility.TryParseHtmlString(Colors.ColorArray[_count], out m_TargetCurrentColor);
        m_TargetIcon.color = m_TargetCurrentColor;
    }
}
