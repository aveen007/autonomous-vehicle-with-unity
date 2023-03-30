using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype : MonoBehaviour
{
    [SerializeField] private Rigidbody _carRb;
    [SerializeField] private Transform _wheelTransform;
    [SerializeField] private Transform _carTransform;
    [SerializeField] private float _strength = 1000;
    [SerializeField] private float _damping= 100;
    [SerializeField] private float _desiredDistance;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _wheelMass;
    [SerializeField] private float _gripFactor;
    [SerializeField] private float _frictionFactor;
    public float throttleSpeed;
    public float steerSpeed;
    public float brakeForce;
    public float slipForce;

    public float CarTopSpeed;

    public AnimationCurve powerCurve;
    public AnimationCurve frictionCurve;

    [SerializeField] private int wheelDirection = 1;

    private void FixedUpdate()
    {
        //suspension force
        float suspensionRestDist = _desiredDistance;
      
        RaycastHit hit;
        Ray ray = new Ray(_wheelTransform.position, -1 * _wheelTransform.up);
        if (Physics.Raycast(ray, out hit, _maxDistance))
        {
            Vector3 springDir = _wheelTransform.up;
            springDir.z = 0;
            Vector3 carVel = _carRb.GetPointVelocity(_wheelTransform.position);
            float distance = hit.distance;
            float offset = suspensionRestDist - distance;
            float vel = Vector3.Dot(springDir, carVel);
            float force = (_strength * offset) - (vel * _damping);
      
            _carRb.AddForceAtPosition(springDir * force, _wheelTransform.position);
    
                CalcSteer();
      
           
                CalcAcceleration();
            
 

           //     CalcFriction();
     
        }
    }

    void CalcSteer()
    {

        Vector3 steerDir = _wheelTransform.right * wheelDirection;
        steerDir.y = 0;
        Vector3 tireWorldVel = _carRb.GetPointVelocity(_wheelTransform.position);
        tireWorldVel.y = 0;
       
        float steerVel = Vector3.Dot(steerDir, tireWorldVel);
        float desiredVelChange = -steerVel * _gripFactor;
        float desiredAcceleration = desiredVelChange / Time.fixedDeltaTime;
        Debug.DrawRay(_wheelTransform.position, steerDir * _wheelMass * desiredAcceleration, Color.red);
        _carRb.AddForceAtPosition(steerDir * _wheelMass * desiredAcceleration, _wheelTransform.position);

    }

    void CalcAcceleration()
    {
        Vector3 accelerateDir = _wheelTransform.forward;
      
        Vector3 carVel = _carRb.velocity;
      
        float carSpeed = Vector3.Dot(accelerateDir, carVel);

        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / CarTopSpeed);
        float availableTorque = powerCurve.Evaluate(normalizedSpeed) * throttleSpeed* CarTopSpeed;

       
          Debug.DrawRay(_carTransform.position, accelerateDir * availableTorque,Color.green);
        _carRb.AddForceAtPosition(accelerateDir * availableTorque, _wheelTransform.position);



    }
    void CalcFriction()
    {
        Vector3 frictionDir = -_carTransform.forward;

        float carSpeed = Vector3.Dot(_wheelTransform.forward, _carRb.velocity);

        
        float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / CarTopSpeed);
        float availableTorque = frictionCurve.Evaluate(normalizedSpeed) * carSpeed * _frictionFactor;
        Vector3 frictionPos = _wheelTransform.position + new Vector3(0f, -0.38f, 0f);
  
        Debug.DrawRay(frictionPos, frictionDir * availableTorque,Color.blue);
        _carRb.AddForceAtPosition(frictionDir * availableTorque, _wheelTransform.position);



    }
}
