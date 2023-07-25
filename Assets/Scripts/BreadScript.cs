using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadScript : MonoBehaviour
{
    private bool _toBeDestroyed = false;
    private float _timer = 0;
    private const float MAX_TIME = 10f;

    private void Update()
    {
        if (_toBeDestroyed)
        {
            _timer += Time.deltaTime;
            if (_timer > MAX_TIME)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("BreadContainer"))
        {
            _toBeDestroyed = false;
            _timer = 0;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("BreadContainer"))
        {
            _toBeDestroyed = true;
        }
    }
}
