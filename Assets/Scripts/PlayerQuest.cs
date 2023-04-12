using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour
{
    private Rigidbody _rgbd;

    private bool _returningQuest;
    private float _currentSpeed;

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _currentSpeed = _rgbd.velocity.magnitude;

        if (_returningQuest && _currentSpeed <= 0.01f)
        {
            Debug.Log("Finish quest!");
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "QuestCircle")
            _returningQuest = true;
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "QuestCircle")
            _returningQuest = false;
    }
}
