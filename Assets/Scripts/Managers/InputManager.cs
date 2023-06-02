using UnityEngine.EventSystems;
using System;
using UnityEngine;

public class InputManager : CustomBehaviour
{
    #region Attributes
    private bool m_IsUIOverride;
    [SerializeField] private bool m_CanClickable;
    #endregion

    #region Events
    public event Action OnClicked;
    public event Action<bool> OnChangedCanClicked;
    #endregion

    public override void Initialize()
    {
        m_CanClickable = false;
        OnClicked += SetMatchableRay;
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnEnterEvent += OnGameStartEnter;
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnExitEvent += OnGameStartExit;
    }
    private void Update()
    {
        UpdateUIOverride();
        UpdateInput();
    }
    public void UpdateInput()
    {
        if (!m_IsUIOverride)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke();
            }
        }
    }

    public void UpdateUIOverride()
    {

#if UNITY_EDITOR
        m_IsUIOverride = EventSystem.current.IsPointerOverGameObject();
#else
        m_IsUIOverride = EventSystem.current.IsPointerOverGameObject(0);
#endif
    }
    private RaycastHit m_MatchableHit;
    private Ray m_MatchableRay;
    private int m_CubeLayerMask = 1 << (int)ObjectsLayer.Matchable;
    private Matchable m_ClickedMatchable;
    private void SetMatchableRay()
    {
        if (!m_CanClickable)
        {
            return;
        }
        m_MatchableRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(m_MatchableRay, out m_MatchableHit, Mathf.Infinity, m_CubeLayerMask))
        {
            SetCanClickable(false);
            m_ClickedMatchable = m_MatchableHit.transform.GetComponent<Matchable>();
            m_ClickedMatchable.ClickedMatchable();
        }
    }
    public void SetCanClickable(bool _clickable)
    {
        m_CanClickable = _clickable;
        OnChangedCanClicked?.Invoke(m_CanClickable);
    }
    #region Events
    private void OnGameStartEnter()
    {
        SetCanClickable(true);
    }
    private void OnGameStartExit()
    {
        SetCanClickable(false);
    }
    private void OnDestroy()
    {
        OnClicked -= SetMatchableRay;
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnEnterEvent -= OnGameStartEnter;
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnExitEvent -= OnGameStartExit;
    }
    #endregion
}
