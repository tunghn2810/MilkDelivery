using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillCircle : MonoBehaviour
{
    [SerializeField] private GameObject _bakeryPanel;

    private void OnTriggerEnter(Collider collider)
    {
        _bakeryPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider collider)
    {
        _bakeryPanel.SetActive(false);
    }
}
