using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapControl : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    private Vector3 _camOffset = new Vector3(0, 200f, 0);
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerPin;

    private Vector3 _followPos;
    private Quaternion _followRot;

    private void Update()
    {
        _followPos = new Vector3(_player.position.x, _playerPin.position.y, _player.position.z);
        _playerPin.position = _followPos;
        _cam.position = _followPos + _camOffset;

        _followRot = new Quaternion(0, _player.rotation.y, 0, _player.rotation.w);
        _cam.rotation = _followRot;
    }
}
