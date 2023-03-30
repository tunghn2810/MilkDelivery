using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics2 : MonoBehaviour
{
    //References
    private Rigidbody _rgbd;
	[SerializeField] private Transform _wheelTransform;
    
    //Wheel classification
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
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _brakeStrength;
    private float _currentSpeed;
    private float _normalizedSpeed;
    private float _availableTorque;
    private Vector3 _accelForce;
    private float _reverseSpeed;
    private Vector3 _brakeForce;

    //Inputs
	private float _steerAngle;
    public float SteerAngle { get => _steerAngle; set => _steerAngle = value; }
    private float _accelDirection;
    public float AccelDirection { get => _accelDirection; set => _accelDirection = value; }
    private bool _isBraking;
    public bool IsBraking { get => _isBraking; set => _isBraking = value; }

    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RotateWheel();
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
			_rgbd.AddForceAtPosition(_suspensionForce, hit.point);

            //Steering force
            _steeringVelocity = Vector3.Dot(transform.right, _worldVelocity);
            _steeringAccel = (-_steeringVelocity * _tireGripFactor) / Time.fixedDeltaTime;
            _steeringForce = transform.right * _tireMass * _steeringAccel;
            _rgbd.AddForceAtPosition(_steeringForce, hit.point);

            //Acceleration force
            _currentSpeed = Vector3.Dot(_rgbd.transform.forward, _rgbd.velocity);
            _normalizedSpeed = Mathf.Clamp01(Mathf.Abs(_currentSpeed) / _maxSpeed);
            _availableTorque = _accelCurve.Evaluate(_normalizedSpeed) * _accelDirection;
            _accelForce = transform.forward * _availableTorque;
            _rgbd.AddForceAtPosition(_accelForce, hit.point);

            //Brake force
            if (_isBraking && _currentSpeed > 0)
            {
                //_reverseSpeed = Vector3.Dot(-_rgbd.transform.forward, _rgbd.velocity);
                _brakeForce = -_rgbd.velocity * _brakeStrength;
                _rgbd.AddForceAtPosition(_brakeForce, hit.point);
            }
        }
	}

    private void RotateWheel()
	{
		_wheelAngle = Mathf.Lerp(_wheelAngle, _steerAngle, _steerTime * Time.deltaTime);
        _wheelTransform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + _wheelAngle, transform.localRotation.z);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + _wheelAngle, transform.localRotation.z);
	}
}
