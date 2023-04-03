using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics1 : MonoBehaviour
{
	//References
	private Rigidbody _rgbd;
	[SerializeField] private Transform _wheelTransform;


	//Wheels
	[SerializeField] private bool _frontLeftWheel;
    [SerializeField] private bool _frontRightWheel;
    [SerializeField] private bool _rearLeftWheel;
    [SerializeField] private bool _rearRightWheel;
    public bool FrontLeftWheel { get => _frontLeftWheel; }
    public bool FrontRightWheel { get => _frontRightWheel; }
    public bool RearLeftWheel { get => _rearLeftWheel; }
    public bool RearRightWheel { get => _rearRightWheel; }
	[SerializeField] float _wheelRadius;
	private Vector3 _totalForce;


    [Header("Suspension")]
	[SerializeField] private float _restLength;
	[SerializeField] private float _springTravel;
	[SerializeField] private float _springStiffness;
	[SerializeField] private float _springDamping;
	private float _minLength;
	private float _maxLength;
	private float _lastLength;
	private float _springLength;
	private float _springVelocity;
	private float _springForce;
    private float _dampingForce;
    private Vector3 _suspensionForce;


	[Header("Steering")]
    [SerializeField] private float _steerTime;
	private float _wheelAngle;


	[Header("Acceleration")]
	[SerializeField] private float _accelRate;
	[SerializeField] private float _maxSpeed;
	[SerializeField] private float _brakeStrength;
	[SerializeField] private float _currentSpeed;
	private Vector3 _localVelocity;
	private float _forceForward;
	private float _forceLeft;
	private Vector3 _accelForce;
	private float _forceBackward;
	private float _forceRight;
	private Vector3 _brakeForce;
	private Vector3 _rollingForce;


	//Inputs
	private float _steerAngle;
    public float SteerAngle { get => _steerAngle; set => _steerAngle = value; }
	private float _accelDirection = 0;
	public float AccelDirection { get => _accelDirection; set => _accelDirection = value; }
    private bool _isBraking;
    public bool IsBraking { get => _isBraking; set => _isBraking = value; }
	private float _brakeMultiplier;
	public float BrakeMultiplier { get => _brakeMultiplier; set => _brakeMultiplier = value; }


    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();

		_minLength = _restLength - _springTravel;
        _maxLength = _restLength + _springTravel;
    }

    private void Update()
    {
		RotateWheel();
    }

    private void FixedUpdate()
    {
		if (_rgbd.velocity.magnitude < 0.01f && _accelDirection == 0f)
        {
            _rgbd.velocity = Vector3.zero;
        }
        _rgbd.velocity = Vector3.ClampMagnitude(_rgbd.velocity, _maxSpeed);
		Force();
    }

	private void Force()
	{
		if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength + _wheelRadius))
		{
			//Suspension force
			_lastLength = _springLength;
			_springLength = hit.distance - _wheelRadius;
			_springLength = Mathf.Clamp(_springLength, _minLength, _maxLength);
			
			_springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;
			
			_springForce = _springStiffness * (_restLength - _springLength);
			_dampingForce = _springDamping * _springVelocity;
			
			_suspensionForce = (_springForce + _dampingForce) * transform.up;

			//_rgbd.AddForceAtPosition(_suspensionForce, hit.point);

			//Acceleration force
            _localVelocity = transform.InverseTransformDirection(_rgbd.GetPointVelocity(hit.point));
            _forceForward = _accelDirection * _accelRate * _springForce;
            _forceLeft = _localVelocity.x * _springForce;

			_accelForce = _forceForward * transform.forward + _forceLeft * -transform.right;

            //_rgbd.AddForceAtPosition(_accelForce, hit.point);
			
			//Speed
			_currentSpeed = Vector3.Dot(_rgbd.transform.forward, _rgbd.velocity);

			//Brake force
            if (_isBraking && Mathf.Abs(_currentSpeed) > 0)
            {
				_forceBackward = _brakeStrength * _springForce;

				_brakeForce = _forceBackward * -_rgbd.velocity.normalized;
                //_brakeForce = -_rgbd.velocity.normalized * _brakeStrength;
                //_rgbd.AddForceAtPosition(_brakeForce, hit.point);
            }
			else
			{
				_brakeForce = Vector3.zero;
			}

			//Rolling drag force
            if (_accelDirection == 0)
            {
                _rollingForce = -_rgbd.velocity.normalized * _brakeStrength/10f;
                //_rgbd.AddForceAtPosition(_rollingForce, hit.point);
            }
			else
			{
				_rollingForce = Vector3.zero;
			}

			_totalForce = _suspensionForce + _accelForce + _brakeForce + _rollingForce;
			_rgbd.AddForceAtPosition(_totalForce, hit.point);
        }
	}

	private void RotateWheel()
	{
		_wheelAngle = Mathf.Lerp(_wheelAngle, _steerAngle, _steerTime * Time.deltaTime);
        _wheelTransform.localRotation = Quaternion.Euler(Vector3.up * _wheelAngle);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + _wheelAngle, transform.localRotation.z);
	}

	private void SpinWheel()
	{
		
	}
}
