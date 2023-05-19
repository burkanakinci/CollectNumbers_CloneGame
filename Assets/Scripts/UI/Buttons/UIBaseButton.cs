using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class UIBaseButton<T> : CustomBehaviour<T>
{
    protected Action m_ButtonClickAction;
    [SerializeField] protected Button m_Button;
    public override void Initialize(T _cachedComponent)
    {
        base.Initialize(_cachedComponent);
        AddFunctionToButtonListener(OnClickAction);
    }

    protected abstract void OnClickAction();

    public void AddFunctionToButtonListener(Action _clickAction)
    {
        m_ButtonClickAction += _clickAction;
        m_Button.onClick.AddListener(ButtonClick);
    }

    protected virtual void OnDestroy()
    {
        m_Button.onClick.RemoveAllListeners();
    }

    protected virtual void ButtonClick()
    {
        m_ButtonClickAction?.Invoke();
    }
}
