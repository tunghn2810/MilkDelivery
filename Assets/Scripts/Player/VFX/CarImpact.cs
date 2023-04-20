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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (Mathf.Abs(_carController.CurrentSpeed) > 20f && !_impacted)
            {
                _starImpact.transform.position = collision.contacts[0].point;
                _starImpact.GetComponent<StarImpact>().PointAtPlayer(transform);
                _starImpact.GetComponent<ParticleSystem>().Play();
                _impacted = true;
            }
        }
    }
}
