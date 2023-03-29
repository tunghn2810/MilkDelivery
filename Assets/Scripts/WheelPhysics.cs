using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : MonoBehaviour
{
	private Rigidbody _rgbd;
	[SerializeField] private Transform _wheelTransform;

	[SerializeField] private bool _frontLeftWheel;
    [SerializeField] private bool _frontRightWheel;
    [SerializeField] private bool _rearLeftWheel;
    [SerializeField] private bool _rearRightWheel;
    public bool FrontLeftWheel { get => _frontLeftWheel; }
    public bool FrontRightWheel { get => _frontRightWheel; }
    public bool RearLeftWheel { get => _rearLeftWheel; }
    public bool RearRightWheel { get => _rearRightWheel; }

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

	[Header("Wheel")]
	private float _steerAngle;
    public float SteerAngle { get => _steerAngle; set => _steerAngle = value; }
	private float _wheelAngle;
    [SerializeField] private float _steerTime;
	private Vector3 _wheelVelocityLocal;
	private float _forceForward;
	private float _forceLeft;
	private float _accelDirection = 0;
	public float AccelDirection { get => _accelDirection; set => _accelDirection = value; }
	private float _brakeMultiplier;
	public float BrakeMultiplier { get => _brakeMultiplier; set => _brakeMultiplier = value; }
	private Vector3 _accelForce;
	[SerializeField] private float _accelRate;
	private float _speedLimiter = 1f;
	[SerializeField] private float _maxSpeed;
	[SerializeField, Range(0f, 1f)] private float _drag = 0.99f;


    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();

		_minLength = _restLength - _springTravel;
        _maxLength = _restLength + _springTravel;
    }

    private void Update()
    {
		SpeedLimit();
		RotateWheel();
    }

    private void FixedUpdate()
    {
		Force();
    }

	private void Force()
	{
		if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength))
		{
			//Suspension force
			_lastLength = _springLength;
			_springLength = hit.distance;
			_springLength = Mathf.Clamp(_springLength, _minLength, _maxLength);
			
			_springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;
			
			_springForce = _springStiffness * (_restLength - _springLength);
			_dampingForce = _springDamping * _springVelocity;
			
			_suspensionForce = (_springForce + _dampingForce) * transform.up;

			_rgbd.AddForceAtPosition(_suspensionForce, transform.position);

			//Acceleration force
            _wheelVelocityLocal = transform.InverseTransformDirection(_rgbd.GetPointVelocity(hit.point));
            _forceForward = _accelDirection * _accelRate * _speedLimiter;
            _forceLeft = _wheelVelocityLocal.x * _accelRate/2 * _brakeMultiplier;

			_accelForce = _forceForward * transform.forward + _forceLeft * -transform.right;

            _rgbd.AddForceAtPosition(_accelForce, transform.position);
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
		
	}

	private void SpeedLimit()
	{
		_speedLimiter = _rgbd.velocity.magnitude >= 20f ? 0f : 1f;
	}
}
