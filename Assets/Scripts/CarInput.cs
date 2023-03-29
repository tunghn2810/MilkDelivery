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
        _carControls.Car.Accelerate.canceled += Accelerate;
        _carControls.Car.Steer.performed += Steer;
        _carControls.Car.Steer.canceled += Steer;
        _carControls.Car.Drift.performed += Drift;
        _carControls.Car.Drift.canceled += Drift;
    }

    public void Accelerate(InputAction.CallbackContext context)
    {
        _carController.Accelerate(context.ReadValue<Vector2>());
    }

    public void Steer(InputAction.CallbackContext context)
    {
        _carController.Steer(context.ReadValue<Vector2>());
    }

    public void Drift(InputAction.CallbackContext context)
    {
        _carController.Drift(context.performed);
    }
}
