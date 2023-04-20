using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraControl : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cineCam;
    //private Cinemachine3rdPersonFollow _cineFollow;
    private CinemachineTransposer _cineTrans;

    ////Third person camera
    //private float _defaultOffset = 2.2f;
    //private float _obsOffset = 7.3f;
    //private float _defaultCamDist = 4f;
    //private float _obsCamDist = 2f;

    //Transposer
    [SerializeField] private float _defaultOffsetZ;
    [SerializeField] private float _obsOffsetZ;
    [SerializeField] private float _defaultOffsetY;
    [SerializeField] private float _obsOffsetY;
    [SerializeField] private float _adjustTime;

    private bool _isBlockedBehind = false;

    //Obstacle
    private GameObject _obstacle;
    private Vector3 _obsPosition;
    private GameObject _closestObstacle;

    private void Awake()
    {
        //_cineFollow = _cineCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        _cineTrans = _cineCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        MoveCameraUp();
    }

    private void MoveCameraUp()
    {
        if (_isBlockedBehind)
        {
            _cineTrans.m_FollowOffset.z = Mathf.Lerp(_cineTrans.m_FollowOffset.z, _obsOffsetZ, _adjustTime * Time.deltaTime);

            _obsOffsetY = _obstacle.GetComponent<MeshRenderer>().bounds.extents.y * 2f;
            _cineTrans.m_FollowOffset.y = Mathf.Lerp(_cineTrans.m_FollowOffset.y, _obsOffsetY, _adjustTime * Time.deltaTime);
        }
        else if (!_isBlockedBehind)
        {
            _cineTrans.m_FollowOffset.z = Mathf.Lerp(_cineTrans.m_FollowOffset.z, _defaultOffsetZ, _adjustTime * Time.deltaTime);

            _cineTrans.m_FollowOffset.y = Mathf.Lerp(_cineTrans.m_FollowOffset.y, _defaultOffsetY, _adjustTime * Time.deltaTime);
        }
    }

    //Third person camera
    //private void MoveCameraUp()
    //{
    //    if (_isBlockedBehind && _cineFollow.ShoulderOffset.y < _obsOffset)
    //    {
    //        _cineFollow.ShoulderOffset.y = Mathf.Lerp(_cineFollow.ShoulderOffset.y, _obsOffset, 0.1f);
    //        _cineFollow.CameraDistance = Mathf.Lerp(_cineFollow.CameraDistance, _obsCamDist, 0.1f);;
    //    }
    //    else if (!_isBlockedBehind && _cineFollow.ShoulderOffset.y > _defaultOffset)
    //    {
    //        _cineFollow.ShoulderOffset.y = Mathf.Lerp(_cineFollow.ShoulderOffset.y, _defaultOffset, 0.1f);
    //        _cineFollow.CameraDistance = Mathf.Lerp(_cineFollow.CameraDistance, _defaultCamDist, 0.1f);;
    //    }
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = true;
            _obstacle = collider.gameObject;
            _obsPosition = _obstacle.transform.position;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = true;
            _obstacle = collider.gameObject;
            _obsPosition = _obstacle.transform.position;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            _isBlockedBehind = false;
            _obstacle = null;
            _obsPosition = Vector3.zero;
        }
    }
}
