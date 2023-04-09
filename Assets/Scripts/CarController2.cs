using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    [SerializeField] private float _centerOfMassOffset;
    public WheelPhysics2[] _wheels;

    private float _steerAngle;
    private float _steerInput;
    private Vector2 _accelDirection;
    private bool _isBraking;
    [SerializeField, Range(0, 1)] private float _turnCurve;

    private Vector3 _centerOfMass;
    [SerializeField] private GameObject _milkBox;

    private void Awake()
    {
        _centerOfMass = GetComponent<Rigidbody>().centerOfMass - new Vector3(0, _centerOfMassOffset, 0);
        _milkBox.SetActive(true);
        GetComponent<Rigidbody>().centerOfMass = _centerOfMass;
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
}
