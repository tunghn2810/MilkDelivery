using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarImpact : MonoBehaviour
{
    [SerializeField] private GameObject _starImpact;
    private CarController2 _carController;

    //Timer - So that you don't spam particle by grinding at the wall at high speed
    private float _timer;
    private float _cooldown = 0.5f;
    private bool _impacted = false;


    //Water timer - in case you can't get up
    private float _waterTimer = 0;
    private float _waterCD = 5f;
    private int _waterCount = 0;
    private bool _waterFirstTime = false;


    private void Awake()
    {
        _carController = GetComponent<CarController2>();
    }

    private void Update()
    {
        if (_impacted)
        {
            _timer += Time.deltaTime;
            if (_timer >= _cooldown)
            {
                _timer = 0;
                _impacted = false;
            }
        }

        if (_waterFirstTime)
        {
            _waterTimer += Time.deltaTime;
            if (_waterTimer >= _waterCD)
            {
                _waterTimer = 0;
                _waterFirstTime = false;
            }
        }

        if (_waterTimer < _waterCD && _waterCount > 5)
        {
            transform.position = new Vector3(44f, 30f, 9f); //Next to bakery
            _waterTimer = 0;
            _waterFirstTime = false;
            _waterCount = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Star impact
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        //{
        //    if (Mathf.Abs(_carController.CurrentSpeed) > 20f && !_impacted)
        //    {
        //        _starImpact.transform.position = collision.contacts[0].point;
        //        _starImpact.GetComponent<StarImpact>().PointAtPlayer(transform);
        //        _starImpact.GetComponent<ParticleSystem>().Play();
        //        _impacted = true;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Water collision
        if (collider.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            _carController.ResetPosition();

            if (!_waterFirstTime)
            {
                _waterFirstTime = true;
            }
            else
            {
                _waterCount++;
            }
        }
    }
}
