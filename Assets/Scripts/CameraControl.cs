using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cineCam;
    private Cinemachine3rdPersonFollow _cineFollow;

    private float _defaultOffset = 2.2f;
    private float _obsOffset = 7.3f;

    private float _defaultCamDist = 4f;
    private float _obsCamDist = 2f;

    private bool _isBlockedBehind = false;

    private void Awake()
    {
        _cineFollow = _cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    private void Update()
    {
        MoveCameraUp();
    }

    private void MoveCameraUp()
    {
        if (_isBlockedBehind && _cineFollow.ShoulderOffset.y < _obsOffset)
        {
            _cineFollow.ShoulderOffset.y = Mathf.Lerp(_cineFollow.ShoulderOffset.y, _obsOffset, 0.1f);
            _cineFollow.CameraDistance = Mathf.Lerp(_cineFollow.CameraDistance, _obsCamDist, 0.1f);;
        }
        else if (!_isBlockedBehind && _cineFollow.ShoulderOffset.y > _defaultOffset)
        {
            _cineFollow.ShoulderOffset.y = Mathf.Lerp(_cineFollow.ShoulderOffset.y, _defaultOffset, 0.1f);
            _cineFollow.CameraDistance = Mathf.Lerp(_cineFollow.CameraDistance, _defaultCamDist, 0.1f);;
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
