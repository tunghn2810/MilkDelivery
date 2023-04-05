using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook _cineFreelook;

    private float _topRig = 1f;
    private float _midRig = 0.5f;

    private bool _isBlockedBehind = false;

    private void Update()
    {
        MoveCameraUp();
    }

    private void MoveCameraUp()
    {
        if (_isBlockedBehind && _cineFreelook.m_YAxis.Value < _topRig)
        {
            _cineFreelook.m_YAxis.Value = Mathf.Lerp(_cineFreelook.m_YAxis.Value, _topRig, 0.1f);
        }
        else if (!_isBlockedBehind && _cineFreelook.m_YAxis.Value > _midRig)
        {
            _cineFreelook.m_YAxis.Value = Mathf.Lerp(_cineFreelook.m_YAxis.Value, _midRig, 0.1f);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = false;
        }
    }
}
