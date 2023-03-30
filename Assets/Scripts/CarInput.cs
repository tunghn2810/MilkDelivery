using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private CarController _carController;
    private CarController1 _carController1;
    private CarController2 _carController2;

    private CarControls _carControls; 

    private void Awake()
    {
        _carController = GetComponent<CarController>();
        _carController1 = GetComponent<CarController1>();
        _carController2 = GetComponent<CarController2>();

        _carControls = new CarControls();
        _carControls.Car.Enable();

        _carControls.Car.Accelerate.performed += Accelerate;
        _carControls.Car.Accelerate.canceled += Accelerate;
        _carControls.Car.Steer.performed += Steer;
        _carControls.Car.Steer.canceled += Steer;
        _carControls.Car.Brake.performed += Brake;
        _carControls.Car.Brake.canceled += Brake;
    }

    public void Accelerate(InputAction.CallbackContext context)
    {
        _carController?.HandleAcceleration(context.ReadValue<Vector2>());
        _carController1?.Accelerate(context.ReadValue<Vector2>());
        _carController2?.Accelerate(context.ReadValue<Vector2>());
    }

    public void Steer(InputAction.CallbackContext context)
    {
        _carController?.HandleSteering(context.ReadValue<Vector2>());
        _carController1?.Steer(context.ReadValue<Vector2>());
        _carController2?.Steer(context.ReadValue<Vector2>());
    }

    public void Brake(InputAction.CallbackContext context)
    {
        _carController?.HandleBrake(context.performed);
        _carController1?.Brake(context.performed);
        _carController2?.Brake(context.performed);
    }
}
