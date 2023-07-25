using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _menuCamera;
    [SerializeField] private Camera _playerCamera;

    //Singleton
    public static CameraManager I_CameraManager { get; set; }
    private void Awake()
    {
        if (I_CameraManager == null)
        {
            I_CameraManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchCameraToMenu()
    {
        _menuCamera.enabled = true;
        _playerCamera.enabled = false;
    }

    public void SwitchCameraToPlayer()
    {
        _menuCamera.enabled = false;
        _playerCamera.enabled = true;
    }
}
