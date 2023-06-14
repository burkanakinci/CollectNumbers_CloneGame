using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraManager : CustomBehaviour
{
    [SerializeField] private Camera m_MainCamera;
    public float CameraSize => m_MainCamera.orthographicSize;
    public Vector3 CameraPos => m_MainCamera.gameObject.transform.position;
    public float CameraUpEndPos => CameraSize + CameraPos.y;
    public override void Initialize()
    {
    }
    public void SetCamera()
    {
        SetCameraPosition();
        SetCameraOrtographicSize();
    }
    private void SetCameraPosition()
    {
        m_MainCamera.transform.position = GameManager.Instance.LevelManager.CurrentCameraPosition;
    }
    private float m_TempCameraSize;
    private void SetCameraOrtographicSize()
    {
        m_MainCamera.orthographicSize = GameManager.Instance.LevelManager.CurrentCameraSize;
    }
}
