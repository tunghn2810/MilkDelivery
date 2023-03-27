using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelPhysics : MonoBehaviour
{
	private Rigidbody _rgbd;

	[Header("Suspension")]
	[SerializeField] private float _restLength;
	[SerializeField] private float _springTravel;
	[SerializeField] private float _springStiffness;
	[SerializeField] private float _springDamping;

	[SerializeField] private float _minLength;
	[SerializeField] private float _maxLength;
	[SerializeField] private float _lastLength;
	[SerializeField] private float _springLength;
	[SerializeField] private float _springVelocity;
	[SerializeField] private float _springForce;
    [SerializeField] private float _dampingForce;

    [SerializeField] private Vector3 _suspensionForce;

	[Header("Wheel")]
    [SerializeField] private float _wheelRadius;

    private void Start()
    {
        _rgbd = transform.root.GetComponent<Rigidbody>();

		_minLength = _restLength - _springTravel;
        _maxLength = _restLength + _springTravel;
    }

    private void FixedUpdate()
    {
		SuspensionForce();
    }

	private void SuspensionForce()
	{
		if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, _maxLength + _wheelRadius))
		{
			_lastLength = _springLength;
			_springLength = hit.distance - _wheelRadius;
			_springLength = Mathf.Clamp(_springLength, _minLength, _maxLength);

			_springVelocity = (_lastLength - _springLength) / Time.fixedDeltaTime;

			_springForce = _springStiffness * (_restLength - _springLength);
			_dampingForce = _springDamping * _springVelocity;

			_suspensionForce = (_springForce + _dampingForce) * transform.up;

			_rgbd.AddForceAtPosition(_suspensionForce, hit.point);
		}
	}
}
