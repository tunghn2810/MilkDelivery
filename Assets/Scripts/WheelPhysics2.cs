using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WheelPhysics2 : MonoBehaviour
{
    //References
    private Rigidbody _rgbd;
    private CarController2 _carController;
	[SerializeField] private Transform _wheelTransform;
    [SerializeField] private VisualEffect _smokeEffect;
    [SerializeField] private TrailRenderer _skidMark;
    
    //Wheels
	[SerializeField] private bool _frontLeftWheel;
    [SerializeField] private bool _frontRightWheel;
    [SerializeField] private bool _rearLeftWheel;
    [SerializeField] private bool _rearRightWheel;
    public bool FrontLeftWheel { get => _frontLeftWheel; }
    public bool FrontRightWheel { get => _frontRightWheel; }
    public bool RearLeftWheel { get => _rearLeftWheel; }
    public bool RearRightWheel { get => _rearRightWheel; }


    [Header("Suspension")]
    [SerializeField] private float _raycastLength;

	[SerializeField] private float _restLength;
	[SerializeField] private float _springStrength;
	[SerializeField] private float _springDamping;
    private Vector3 _worldVelocity;
    private float _springOffset;
	private float _springLength;
	private float _springVelocity;
	private float _springForce;
    private float _dampingForce;
    private Vector3 _suspensionForce;


	[Header("Steering")]
    [SerializeField] private float _tireGripFactor;
    [SerializeField] private float _tireMass;
    [SerializeField] private float _steerTime;
    private float _steeringVelocity;
    private float _steeringAccel;
    private Vector3 _steeringForce;
	private float _wheelAngle;


    [Header("Acceleration")]
    [SerializeField] private AnimationCurve _accelCurve;
    //[SerializeField] private float _maxSpeed;
    [SerializeField] private float _accelStrength;
    [SerializeField] private float _brakeStrength;
    [SerializeField] private float _rollingBrakeStrength;
    //[SerializeField] private float _currentSpeed;
    //public float CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
    private float _normalizedSpeed;
    private float _availableTorque;
    private Vector3 _accelForce;
    private float _reverseSpeed;
    private Vector3 _brakeForce;
    private Vector3 _rollingForce;

    [Header("Drifting")]
    [SerializeField] private float _defaultDriftMultiplier;
    [SerializeField] private float _driftingMultiplier;

    //Wheel spin
    private float _spinAngle;

    //Inputs
	[SerializeField] private float _steerAngle;
    public float SteerAngle { get => _steerAngle; set => _steerAngle = value; }
    private float _accelDirection;
    public float AccelDirection { get => _accelDirection; set => _accelDirection = value; }
    private bool _isBraking;
    public bool IsBraking { get => _isBraking; set => _isBraking = value; }
    [SerializeField] private float _driftMultiplier;


    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();
        _carController = transform.root.GetComponent<CarController2>();
    }

    private void Update()
    {
        DriftFactor();
        DriftSmoke();
        RotateWheel();
        SpinWheel();
    }

    private void FixedUpdate()
    {
        Force();
    }

	private void Force()
	{
		if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _raycastLength))
		{
            //World-space velocity of the wheel
            _worldVelocity = _rgbd.GetPointVelocity(transform.position);

			//Suspension force
            _springOffset = _restLength - hit.distance;
            _springVelocity = Vector3.Dot(transform.up, _worldVelocity);
            _springForce = (_springOffset * _springStrength) - (_springVelocity * _springDamping);
			_suspensionForce = _springForce * transform.up;
			_rgbd.AddForceAtPosition(_suspensionForce, transform.position);

            //Steering force
            _steeringVelocity = Vector3.Dot(transform.right, _worldVelocity);
            _steeringAccel = (-_steeringVelocity * _tireGripFactor * _driftMultiplier) / Time.fixedDeltaTime;
            _steeringForce = transform.right * _tireMass * _steeringAccel;
            _rgbd.AddForceAtPosition(_steeringForce, transform.position);

            //Acceleration force
            //_currentSpeed = Vector3.Dot(_rgbd.transform.forward, _rgbd.velocity);
            _normalizedSpeed = Mathf.Clamp01(Mathf.Abs(_carController.CurrentSpeed) / _carController.MaxSpeed);
            _availableTorque = _accelCurve.Evaluate(_normalizedSpeed) * _accelDirection;
            _accelForce = transform.forward * _availableTorque * _accelStrength;
            _rgbd.AddForceAtPosition(_accelForce, transform.position);

            //Brake force
            if (_isBraking && Mathf.Abs(_carController.CurrentSpeed) > 0)
            {
                //_reverseSpeed = Vector3.Dot(-_rgbd.transform.forward, _rgbd.velocity);
                _brakeForce = -_rgbd.velocity.normalized * _brakeStrength * _accelStrength;
                _rgbd.AddForceAtPosition(_brakeForce, transform.position);
            }

            //Rolling drag force
            if (_accelDirection == 0)
            {
                _rollingForce = -_rgbd.velocity.normalized * _rollingBrakeStrength * _accelStrength;
                _rgbd.AddForceAtPosition(_rollingForce, transform.position);
            }
        }
	}

    private void RotateWheel()
	{
		_wheelAngle = Mathf.Lerp(_wheelAngle, _steerAngle, _steerTime * Time.deltaTime);
        _wheelTransform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + _wheelAngle, transform.localRotation.z);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + _wheelAngle, transform.localRotation.z);
	}

    private void SpinWheel()
    {
        _spinAngle += _rgbd.velocity.magnitude;
        //_wheelTransform.Rotate(_spinAngle * Mathf.Sign(_currentSpeed), 0, 0);
        _wheelTransform.localRotation = Quaternion.Euler(_wheelTransform.localRotation.x + _spinAngle, _wheelTransform.localRotation.y, _wheelTransform.localRotation.z);
    }

    private void DriftFactor()
    {
        if (_isBraking)
        {
            _driftMultiplier = Mathf.Lerp(_driftMultiplier, _driftingMultiplier, 5f * Time.deltaTime);
        }
        else
        {
            _driftMultiplier = Mathf.Lerp(_driftMultiplier, _defaultDriftMultiplier, 0.5f * Time.deltaTime);
        }
    }

    private void DriftSmoke()
    {
        if (_rearLeftWheel || _rearRightWheel)
        {
            if (Mathf.Abs(_carController.CurrentSpeed) < 0.1f)
            {
                _smokeEffect.Stop();
                _skidMark.emitting = false;
                return;
            }

            if (_isBraking)
            {
                _smokeEffect.SetVector3("StartPos", transform.position);
                _smokeEffect.Play();
                _skidMark.emitting = true;
            }
            else
            {
                _smokeEffect.Stop();
                _skidMark.emitting = false;
            }
        }
    }
}
