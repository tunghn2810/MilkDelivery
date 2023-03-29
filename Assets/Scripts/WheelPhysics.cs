using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : MonoBehaviour
{
	private Rigidbody _rgbd;
	[SerializeField] private Transform _wheelMesh;

	[SerializeField] private bool _frontLeftWheel;
    [SerializeField] private bool _frontRightWheel;
    [SerializeField] private bool _rearLeftWheel;
    [SerializeField] private bool _rearRightWheel;
    public bool FrontLeftWheel { get => _frontLeftWheel; set => _frontLeftWheel = value; }
    public bool FrontRightWheel { get => _frontRightWheel; set => _frontRightWheel = value; }
    public bool RearLeftWheel { get => _rearLeftWheel; set => _rearLeftWheel = value; }
    public bool RearRightWheel { get => _rearRightWheel; set => _rearRightWheel = value; }

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
	private float _driftingMultiplier;
	public float DriftingMultiplier { get => _driftingMultiplier; set => _driftingMultiplier = value; }
	[SerializeField] private float _speed;


    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();

		_minLength = _restLength - _springTravel;
        _maxLength = _restLength + _springTravel;
    }

    private void Update()
    {
		_wheelAngle = Mathf.Lerp(_wheelAngle, _steerAngle, _steerTime * Time.deltaTime);
        _wheelMesh.localRotation = Quaternion.Euler(Vector3.up * _wheelAngle);
		transform.localRotation = _wheelMesh.localRotation;
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

			//Vector3 tireWorldVel = _rgbd.GetPointVelocity(transform.position);
			//float offset = _restLength - hit.distance;
			//float vel = Vector3.Dot(transform.up, tireWorldVel);
			//
			//_suspensionForce = (offset * _springStiffness - vel * _springDamping) * transform.up;

			_rgbd.AddForceAtPosition(_suspensionForce, transform.position);


			//Acceleration force
            _wheelVelocityLocal = transform.InverseTransformDirection(_rgbd.GetPointVelocity(hit.point));
            _forceForward = _accelDirection * _speed;
            _forceLeft = _wheelVelocityLocal.x * _speed/2 * _driftingMultiplier;

            _rgbd.AddForceAtPosition(_forceForward * transform.forward + _forceLeft * -transform.right, transform.position);
        }
	}
}
