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
        m_MainCamera.transform.position = new Vector3(
            ((GameManager.Instance.LevelManager.CurrentRowCount - 1) * 0.5f),
            ((GameManager.Instance.LevelManager.CurrentColumnCount - 1) * 0.5f),
            Camera.main.transform.position.z
        );
    }
    private float m_TempCameraSize;
    private void SetCameraOrtographicSize()
    {
        if (GameManager.Instance.LevelManager.CurrentColumnCount > GameManager.Instance.LevelManager.CurrentRowCount)
        {
            m_TempCameraSize = 5.0f;
            m_TempCameraSize += (GameManager.Instance.LevelManager.CurrentColumnCount - 4) / 2.0f;
        }
        else
        {
            m_TempCameraSize = GameManager.Instance.LevelManager.CurrentRowCount + 1;
        }
        Camera.main.orthographicSize = m_TempCameraSize;
    }
}
