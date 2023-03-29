using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheelColliders; //Front Left - Front Right - Rear Left - Rear Right
    [SerializeField] private Transform[] _wheelTransforms; //Front Left - Front Right - Rear Left - Rear Right

    private float _accelDirection;
    private float _steerDirection;
    private bool _isBraking;

    private float _steerAngle;
    private float _currentBrakeForce;


    [SerializeField] private float _motorForce;
    [SerializeField] private float _brakeForce;
    [SerializeField] private float _maxSteerAngle;

    private void FixedUpdate()
    {
        ApplyAcceleration();
        ApplySteering();
        ApplyBrake();
        UpdateWheels();
    }

    private void ApplyAcceleration()
    {
        _wheelColliders[0].motorTorque = _accelDirection * _motorForce;
        _wheelColliders[1].motorTorque = _accelDirection * _motorForce;
    }

    private void ApplySteering()
    {
        _steerAngle = _maxSteerAngle * _steerDirection;
        
        _wheelColliders[0].steerAngle = _steerAngle;
        _wheelColliders[1].steerAngle = _steerAngle;
    }

    private void ApplyBrake()
    {
        _currentBrakeForce = _isBraking ? _brakeForce : 0f;
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            _wheelColliders[i].brakeTorque = _currentBrakeForce;
        }
    }

    public void HandleAcceleration(Vector2 accelDirection)
    {
        _accelDirection = accelDirection.y;
    }

    public void HandleSteering(Vector2 steerDirection)
    {
        _steerDirection = steerDirection.x;
    }

    public void HandleBrake(bool isBraking)
    {
        _isBraking = isBraking;
    }

    private void UpdateWheels()
    {
        for (int i = 0; i < _wheelColliders.Length; i++)
        {
            UpdateSingleWheel(_wheelColliders[i], _wheelTransforms[i]);
        }
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
}
