using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform[] _tires;
    private Rigidbody _rgbd;

    [SerializeField, Range(0f, 10f)] float _suspensionRestDist = 1f;
    [SerializeField, Range(0f, 10f)] float _raycastDist = 0.6f;
    [SerializeField, Range(0f, 100f)] float _springStrength = 10f;
    [SerializeField, Range(0f, 100f)] float _springDamper = 8f;

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _tires.Length; i++)
        {
            Force(_tires[i]);
        }
    }

    private void Force(Transform tireTransform)
    {
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(tireTransform.position, Vector3.down, out raycastHit, _raycastDist);

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
    }
}
