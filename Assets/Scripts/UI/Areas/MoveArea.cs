using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveArea : UIArea
{
    [SerializeField] private TextMeshProUGUI m_RemainingMoveText;
    public override void Initialize(UIPanel _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        GameManager.Instance.LevelManager.OnChangeMoveCount += SetRemaingMoveUI;
    }
    public void SetRemaingMoveUI(int _move)
    {
        m_RemainingMoveText.text = _move.ToString();
    }
    private void OnDestroy()
    {
        GameManager.Instance.LevelManager.OnChangeMoveCount -= SetRemaingMoveUI;
    }
}
