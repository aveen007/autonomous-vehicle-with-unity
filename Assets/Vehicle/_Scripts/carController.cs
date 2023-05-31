using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct Wheel
{
    public string name;
    public GameObject mesh;
    public WheelController wheelCollider;
    public enum WheelType
    {
        ForwardWheel,
        RearWheel
    }
    public WheelType wheelType;
}
[RequireComponent(typeof(inputManager))]
[RequireComponent(typeof(Rigidbody))]


public class carController : MonoBehaviour
{
    private float _throtle;// store input throttle
    private float _steer; // store input steer
    private float _brake; // store input brake
    [Header("Car speed and Steering")]
    public float acceleration;
    public float accelRate; // How fast should it gain the max speed
    [Tooltip("the radius of the turn Standard 10.4-10.7m")]
    public float steerRadius=8; // the radius of the turn as experienced by the centerline of the vehicle.
    [Tooltip("Distance between the two axles")]
    public float wheelBase=2.860623f; //the wheelbase of the vehicle (distance between the two axles).
    [Tooltip("Distance between center line of each tyre")]
    public float trackDist = 1.693186f; // the track (distance between center line of each tyre).
    public float steerSpeedRate = 5; //who fast the steer will change
 
    public float brakeFactor; // store input steer
    public AnimationCurve slideCurve;

    public inputManager inputManager;
    [Header("Wheel Configuration")]
    public Wheel[] wheels;
  
    public Rigidbody RB;

    void Start()
    {   
        RB=GetComponent<Rigidbody>();
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.SetSteerParam(wheelBase, steerRadius, trackDist, steerSpeedRate);
        }

    }

    void FixedUpdate()
    {
        _throtle = inputManager.Throttle;
        _steer = inputManager.Steer;
        _brake = inputManager.Brake;
      
        DriveCar();

        if (inputManager.Throttle!=0)
        foreach(var wheel in wheels)
        {
            wheel.mesh.transform.Rotate(RB.velocity.magnitude*(transform.InverseTransformDirection(RB.velocity).z>=0?1:-1)/(2*Mathf.PI*.36f),0f,0f);
         
        }
    }
    public void DriveCar()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.wheelType == Wheel.WheelType.ForwardWheel)
            {
                wheel.wheelCollider.steerSpeed = _steer * steerRadius;
            }
            wheel.wheelCollider.throttleSpeed = _throtle;
            ApplyBrake(wheel);
            ApplySlip(wheel);
        }

    }

    void ApplyBrake(Wheel wheel)
    {
        wheel.wheelCollider.brakeForce =_brake * brakeFactor;
    }

    void ApplySlip(Wheel wheel)
    {
        float slip = Mathf.InverseLerp(0, 200, RB.velocity.magnitude );
        wheel.wheelCollider.slipForce = slideCurve.Evaluate(slip);
      //  Debug.Log(slip);
    }
}
