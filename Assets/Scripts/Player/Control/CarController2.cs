using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    private Rigidbody _rgbd;

    [SerializeField] private float _centerOfMassOffset;
    [SerializeField] private GameObject _massCollider;
    [SerializeField] private GameObject _carCollider;
    public WheelPhysics2[] _wheels;

    //Input
    [SerializeField] private float _steerAngle;
    [SerializeField] private float _steerInput;
    [SerializeField] private Vector2 _accelDirection;
    [SerializeField] private bool _isBraking;

    [SerializeField] private float _maxSpeed;
    public float MaxSpeed { get => _maxSpeed; }
    [SerializeField, Range(0, 1)] private float _turnCurve;

    private Vector3 _centerOfMass;

    private float _currentSpeed;
    public float CurrentSpeed { get => _currentSpeed; }

    [SerializeField] private Vector3 _lastPosition;
    public Vector3 LastPosition { get => _lastPosition; set => _lastPosition = value; }


    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
        _centerOfMass = _rgbd.centerOfMass - new Vector3(0, _centerOfMassOffset, 0);
        _massCollider.SetActive(false);
        _carCollider.SetActive(true);
        //_milkBox.SetActive(true);
        _rgbd.centerOfMass = _centerOfMass;
    }

    private void Update()
    {
        _steerAngle = Mathf.Rad2Deg * Mathf.Atan(_turnCurve) * _steerInput;

        for (int i = 0; i < _wheels.Length; i++)
        {
            if (_wheels[i].FrontLeftWheel || _wheels[i].FrontRightWheel)
            {
                _wheels[i].SteerAngle = _steerAngle;
            }
        }
        
        for (int i = 0; i < _wheels.Length; i++)
        {
            _wheels[i].AccelDirection = _accelDirection.y;
        }

        for (int i = 0; i < _wheels.Length; i++)
        {
            _wheels[i].IsBraking = _isBraking;
        }
    }

    private void FixedUpdate()
    {
        _currentSpeed = Vector3.Dot(_rgbd.transform.forward, _rgbd.velocity);
        ClampVelocity();
    }

    public void Steer(Vector2 steerDir)
    {
        _steerInput = steerDir.x;
    }

    public void Accelerate(Vector2 accelDirection)
    {
        _accelDirection = accelDirection;
    }

    public void Brake(bool isBraking)
    {
        _isBraking = isBraking;
    }

    private void ClampVelocity()
    {
        if (_rgbd.velocity.magnitude < 0.01f)
        {
            _rgbd.velocity = Vector3.zero;
        }
        _rgbd.velocity = Vector3.ClampMagnitude(_rgbd.velocity, _maxSpeed);
    }

    public void ResetPosition()
    {
        transform.position = _lastPosition;
        transform.rotation = Quaternion.identity;
        _rgbd.velocity = Vector3.zero;
        _rgbd.angularVelocity = Vector3.zero;
    }
}
