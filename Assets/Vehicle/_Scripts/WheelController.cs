using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{

    private Transform _wheelTransform;
    private float _finalTurnSpeed;
    [SerializeField] private Rigidbody _carRb;
    [SerializeField] private WheelPos _wheelPos;
    [SerializeField] private Transform _carTransform;
    [SerializeField] private float _strength = 10000;
    [SerializeField] private float _damping = 200;
    [SerializeField] private float _desiredDistance= 0.62525f;
    [SerializeField] private float _maxDistance= 100;
    [SerializeField] private float _wheelMass= 1;
    [SerializeField] private float _gripFactor;
    [SerializeField] private float _frictionFactor=400;
    public float throttleSpeed;
    public float steerSpeed;
    public float brakeForce;
    public float slipForce;
    public float CarTopSpeed= 2000;

    public AnimationCurve powerCurve;
    public AnimationCurve frictionCurve;
    private enum WheelPos
    {
        Left=-1,
        Right=1
    }

   
    private void Start()
    {

        _wheelTransform= transform;
    }
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
            springDir.x = 0;
            Vector3 carVel = _carRb.GetPointVelocity(_wheelTransform.position);
            float distance = hit.distance;
            float offset = suspensionRestDist - distance;
            float vel = Vector3.Dot(springDir, carVel);
            float force = (_strength * offset) - (vel * _damping);
         //   Debug.DrawRay(_wheelTransform.position, springDir * force, Color.red);

            _carRb.AddForceAtPosition(springDir * force, _wheelTransform.position);
            Steer(_wheelBase, _turnRadius, _track);
            CalcSlip();


            CalcAcceleration();




        }
    }




    // Acerman Steering https://datagenetics.com/blog/december12016/index.html
    // L is the wheelbase of the vehicle (distance between the two axles).
    // T is the track (distance between center line of each tyre).
    // R is the radius of the turn as experienced by the centerline of the vehicle.
    private float _wheelBase = 2.860623f;
    private float _track = 1.693186f;
    private float _turnRadius = 10;
    private float _turnSpeedRate = 5;
    public void SetSteerParam(float L, float R, float T, float turnRate)
    {
        _track = T;
        _turnRadius = R;
        _wheelBase = L;
        _turnSpeedRate = turnRate;
    }

    void Steer(float L, float R, float T)
    {
        float insideWheelAngle = Mathf.Rad2Deg * Mathf.Atan2(L, (R - T / 2));
        float outsideWheelAngle = Mathf.Rad2Deg * Mathf.Atan2(L, (R + T / 2));

        switch (_wheelPos)
        {
            case WheelPos.Left:
                _finalTurnSpeed = Mathf.Lerp(_finalTurnSpeed, insideWheelAngle * Sign(steerSpeed), _turnSpeedRate * Time.deltaTime);
                break;
            case WheelPos.Right:
                _finalTurnSpeed = Mathf.Lerp(_finalTurnSpeed, outsideWheelAngle * Sign(steerSpeed), _turnSpeedRate * Time.deltaTime);
                break;
        }
        transform.localRotation = Quaternion.Euler(Vector3.up * _finalTurnSpeed);
    }
    int Sign(float number)
    {
        return number < 0 ? -1 : (number > 0 ? 1 : 0);
    }
    void CalcSlip()
    {

        Vector3 steerDir = _wheelTransform.right * ((float)_wheelPos);
        steerDir.y = 0;
        Vector3 tireWorldVel = _carRb.GetPointVelocity(_wheelTransform.position);
        tireWorldVel.y = 0;

        float steerVel = Vector3.Dot(steerDir, tireWorldVel);
        //Debug.Log(steerVel);
        float desiredVelChange = -steerVel * _gripFactor*slipForce;
        float desiredAcceleration = desiredVelChange / Time.fixedDeltaTime;
       // Debug.DrawRay(_wheelTransform.position, steerDir * _wheelMass * desiredAcceleration, Color.red);
       // Debug.DrawRay(_wheelTransform.position, steerDir * _wheelMass * steerVel, Color.black);
        _carRb.AddForceAtPosition(steerDir * _wheelMass * desiredAcceleration, _wheelTransform.position);

    }

    void CalcAcceleration()
    {
        Vector3 accelerateDir = _wheelTransform.forward;

        Vector3 carVel = _carRb.velocity;
      
            float carSpeed = Vector3.Dot(accelerateDir, carVel);

            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / CarTopSpeed);
            float availableTorque = powerCurve.Evaluate(normalizedSpeed) * throttleSpeed * CarTopSpeed;


            Vector3 accelerationForce = accelerateDir *( availableTorque);
        
     
            Vector3 brakesForce = -1*Sign(carSpeed)* accelerateDir* brakeForce;

        Vector3 frictionForce = -1 * Sign(carSpeed) * accelerateDir * frictionCurve.Evaluate(normalizedSpeed)*_frictionFactor;
        //    Debug.DrawRay(_carTransform.position, frictionForce, Color.cyan);
            _carRb.AddForceAtPosition(brakesForce, _wheelTransform.position);
            _carRb.AddForceAtPosition(frictionForce, _wheelTransform.position);

        
      //  Debug.DrawRay(_carTransform.position, accelerateDir * availableTorque, Color.green);
            _carRb.AddForceAtPosition(accelerationForce, _wheelTransform.position);
        


    }

}


