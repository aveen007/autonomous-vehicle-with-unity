using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using WayPointSystem;

public class WayPointNavigator : MonoBehaviour
{

    TrafficController trafficController;
    WayPoint CurrentWaypoint;
    GameObject WaypointRoot;
    int direction;

    private void Awake()
    {
        trafficController = GetComponent<TrafficController>();

        if (trafficController.agent.agentTypeID == 0)
        {
            WaypointRoot = GameObject.Find("WayPointPedestrian");
            trafficController.agent.speed = UnityEngine.Random.Range(1f, 2f);

        }
        else
        {
            WaypointRoot = GameObject.Find("WayPointRoot");
            trafficController.agent.speed = UnityEngine.Random.Range(10f, 15f);

        }
        CurrentWaypoint = WaypointRoot.transform.GetChild(0).GetComponent<WayPoint>();

    }
    private void Start()
    {
        trafficController.agent.SetDestination(CurrentWaypoint.GetPosition());
        direction = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
        //  trafficController.agent.avoidance
        NavMesh.avoidancePredictionTime = 0.5f;
    }

    private void Update()
    {

        float dist = trafficController.agent.remainingDistance;

        if (dist != Mathf.Infinity && trafficController.agent.pathStatus == NavMeshPathStatus.PathComplete && trafficController.agent.remainingDistance <= 3f)
        {
            bool shouldBransh = false;
            if (CurrentWaypoint.branches != null && CurrentWaypoint.branches.Count > 0)
            {
                shouldBransh = UnityEngine.Random.Range(0f, 1f) <= CurrentWaypoint.branchRatio ? true : false;
            }

            if (shouldBransh)
            {
                CurrentWaypoint = CurrentWaypoint.branches[UnityEngine.Random.Range(0, CurrentWaypoint.branches.Count - 1)];
            }
            if (direction == 0)
            {
                if (CurrentWaypoint.NextWayPointl == null)
                {
                    CurrentWaypoint = CurrentWaypoint.PreviousWayPointl;
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
            Debug.Log(trafficController.agent.velocity.magnitude + "   " + transform.GetSiblingIndex());
            // SolveStuck();
       /*     if (trafficController.agent.velocity.magnitude < 2f && trafficController.agent.hasPath)
            {
                direction=(direction == 0) ? 1 : 0;
                Debug.Log("I'm stuck");
            }
            else
            {*/
                trafficController.agent.SetDestination(CurrentWaypoint.GetPosition());
         //   }
        }

    }
    IEnumerator SolveStuck()
    {
        Vector3 lastPosition = this.transform.position;

        while (true)
        {
            yield return new WaitForSeconds(3f);

            //Maybe we can also use agent.velocity.sqrMagnitude == 0f or similar
            if (!trafficController.agent.pathPending && trafficController.agent.hasPath && trafficController.agent.remainingDistance > trafficController.agent.stoppingDistance)
            {
                Vector3 currentPosition = this.transform.position;
                if (Vector3.Distance(currentPosition, lastPosition) < 1f)
                {
                    Vector3 destination = trafficController.agent.destination;
                    trafficController.agent.ResetPath();
                    trafficController.agent.SetDestination(destination);
                    Debug.Log("Agent Is Stuck");
                }
                Debug.Log("Current Position " + currentPosition + " Last Position " + lastPosition);
                lastPosition = currentPosition;
            }
        }
    }

}