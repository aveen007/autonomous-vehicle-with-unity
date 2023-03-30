using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public inputManager InputManager;
    public float maxTurn = 30f;

    private void Update()
    {
        Execute();
    }

    void Execute()
    {

        float _finalTurnSpeed = maxTurn * InputManager.Steer;
        transform.localRotation = Quaternion.Euler(Vector3.up * _finalTurnSpeed);

    }
}
