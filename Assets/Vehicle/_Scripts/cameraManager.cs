using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using Cinemachine;

public class cameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera WatchVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera CarVirtualCamera;
    private int activeCam ;
    private void Start()
    {
        activeCam = 0;
        
    }
    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.T))
        {
            activeCam = (activeCam + 1) % 2;

        }
      //  Debug.Log("active Cam" + activeCam);
       if(activeCam == 0)
        {
            WatchVirtualCamera.Priority = 11;
            CarVirtualCamera.Priority = 10;
        }
        else
        {
            WatchVirtualCamera.Priority = 10;
            CarVirtualCamera.Priority = 11;
        }
    }
}
