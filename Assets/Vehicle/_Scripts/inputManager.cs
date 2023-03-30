using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputManager : MonoBehaviour
{
    public float Throttle;
    public float Steer;
    public float Brake;

    private void Update()
    {
        Throttle = Input.GetAxis("Vertical");
        Steer = Input.GetAxis("Horizontal");
        Brake = Input.GetAxis("Jump");

    }
}
