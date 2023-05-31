using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WayPointSystem;

public class WayPointNavigator : MonoBehaviour
{

    OtherCarController otherCarController;
    WayPoint CurrentWaypoint;
    GameObject WaypointRoot;
    int direction;
    
    private void Awake()
    {
        otherCarController = GetComponent<OtherCarController>();
        WaypointRoot = GameObject.Find("WayPointRoot");
        CurrentWaypoint= WaypointRoot.transform.GetChild(0).GetComponent<WayPoint>();
        otherCarController.agent.speed = UnityEngine.Random.Range(10f, 15f);
    }
    private void Start()
    {
        otherCarController.agent.SetDestination(CurrentWaypoint.GetPosition());
        direction=Mathf.RoundToInt(UnityEngine.Random.Range(0f,1f));
    }

    private void Update()
    {

        float dist = otherCarController.agent.remainingDistance;
     
        if (dist != Mathf.Infinity && otherCarController.agent.pathStatus == NavMeshPathStatus.PathComplete && otherCarController.agent.remainingDistance <= 3f)
        {
            bool shouldBransh = false;
            if (CurrentWaypoint.branches != null&&CurrentWaypoint.branches.Count>0)
            {
                shouldBransh=UnityEngine.Random.Range(0f,1f)<=CurrentWaypoint.branchRatio?true:false;
            }

            if (shouldBransh)
            {
                CurrentWaypoint = CurrentWaypoint.branches[UnityEngine.Random.Range(0, CurrentWaypoint.branches.Count - 1)];
            }
            if (direction == 0)
            {
                if (CurrentWaypoint.NextWayPointl == null)
                {
                    CurrentWaypoint=CurrentWaypoint.PreviousWayPointl;
                    direction = 1;
                }
                else
                {
                    CurrentWaypoint = CurrentWaypoint.NextWayPointl;
                }
            }
            else
            {
                if (CurrentWaypoint.PreviousWayPointl == null)
                {
                    CurrentWaypoint = CurrentWaypoint.NextWayPointl;
                    CurrentWaypoint = CurrentWaypoint.NextWayPointl;
                    direction = 0;
                }
                else
                {
                    
                    CurrentWaypoint = CurrentWaypoint.PreviousWayPointl;
                }
            }
            otherCarController.agent.SetDestination(CurrentWaypoint.GetPosition());

        }
    }
}
