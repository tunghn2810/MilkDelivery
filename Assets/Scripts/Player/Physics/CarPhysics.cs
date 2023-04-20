using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    [SerializeField] private Transform[] _tires;
    private Rigidbody _rgbd;

    float _suspensionRestDist = 1f;
    float _springStrength = 10f;
    float _springDamper = 8f;

    float _tireGripFactor = 0.5f;
    float _tireMass = 0.5f;

    float _accelInput;
    float _carTopSpeed = 10f;

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _accelInput = Input.GetAxis("Vertical");

        for (int i = 0; i < _tires.Length; i++)
        {
            Force(_tires[i]);
        }
    }

    private void Force(Transform tireTransform)
	{
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(tireTransform.position, Vector3.down, out raycastHit, 0.6f);

        //Spring force
        if (hit)
        {
            //World space direction of spring force
            Vector3 springDir = tireTransform.up;

            //World space velocity of this tire
            Vector3 tireWorldVel = _rgbd.GetPointVelocity(tireTransform.position);

            //Calculate offset from the raycast
            float offset = _suspensionRestDist - raycastHit.distance;

            //Calculate velocity along the spring direction
            float vel = Vector3.Dot(springDir, tireWorldVel);

            //Calculate the magnitude of dampened spring force
            float force = (offset * _springStrength) - (vel * _springDamper);

            Debug.DrawRay(tireTransform.position, springDir * force, Color.green);

            //Apply the force at the location of the tire, in the direction of the suspension
            _rgbd.AddForceAtPosition(springDir * force, tireTransform.position);
        }

        //Steering force
        if (hit)
        {
            //World space direction of steering force
            Vector3 steeringDir = tireTransform.right;

            //World space velocity of the suspension
            Vector3 tireWorldVel = _rgbd.GetPointVelocity(tireTransform.position);

            //Calculate tire velocity in the steering direction
            float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

            //Change in velocity
            //gripFactor is in range 0 - 1, 0 = no grip, 1 = full grip
            float desiredVelChange = -steeringVel * _tireGripFactor;

            //Turn change in velocity into acceleration
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

            Debug.DrawRay(tireTransform.position, steeringDir * _tireMass * desiredAccel, Color.red);

            //Apply the force at the location of the tire, in the direction of the suspension
            _rgbd.AddForceAtPosition(steeringDir * _tireMass * desiredAccel, tireTransform.position);
        }

        //Acceleration / braking
        if (hit)
        {
            //World space direction of acceleration/braking force
            Vector3 accelDir = tireTransform.forward;

            //Acceleration torque
            if (_accelInput > 0f)
            {
                //Forward speed of the car
                float carSpeed = Vector3.Dot(transform.forward, _rgbd.velocity);

                //Normalized car speed
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / _carTopSpeed);

                //Available torque
                float availableTorque = 1f
                    * _accelInput;

                _rgbd.AddForceAtPosition(accelDir * availableTorque, tireTransform.position);
            }
        }
    }
}
