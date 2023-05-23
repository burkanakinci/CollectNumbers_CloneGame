using UnityEngine.EventSystems;
using System;
using UnityEngine;

public class InputManager : CustomBehaviour
{
    #region Attributes
    private bool m_IsUIOverride;
    [SerializeField] private bool m_CanClickable;
    #endregion

    public override void Initialize()
    {
        m_CanClickable = false;
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
                SetMatchableRay();
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
            m_CanClickable = false;
            m_ClickedMatchable = m_MatchableHit.transform.GetComponent<Matchable>();
            m_ClickedMatchable.ClickedMatchable();
        }
    }
    #region Events
    private void OnGameStartEnter()
    {
        m_CanClickable = true;
    }
    private void OnGameStartExit()
    {
        m_CanClickable = false;
    }
    private void OnDestroy()
    {
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnEnterEvent -= OnGameStartEnter;
        GameManager.Instance.PlayerManager.Player.PlayerStateMachine.GetPlayerState(PlayerStates.RunState).OnExitEvent -= OnGameStartExit;
    }
    #endregion
}
