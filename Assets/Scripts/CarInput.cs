using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private CarController _carController;

    private CarControls _carControls; 

    private void Awake()
    {
        _carController = GetComponent<CarController>();

        _carControls = new CarControls();
        _carControls.Car.Enable();

        _carControls.Car.Accelerate.performed += Accelerate;
        _carControls.Car.Steer.performed += Steer;
    }

    public void Accelerate(InputAction.CallbackContext context)
    {
        Debug.Log("Accel");
    }

    public void Steer(InputAction.CallbackContext context)
    {
        Debug.Log("Steer" + context.ReadValue<Vector2>());
    }
}
