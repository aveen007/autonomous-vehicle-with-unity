using UnityEngine.AI;
using UnityEngine;

public class OtherCarController : MonoBehaviour
{
   // public Camera cam;
    public NavMeshAgent agent;
    public delegate void OnDisableCallBack(OtherCarController otherCarController);
    public OnDisableCallBack Disable;
    void Update()
    {
        /*   if (Input.GetMouseButtonDown(0))
           {
               Ray ray = cam.ScreenPointToRay(Input.mousePosition);
               RaycastHit hit;

               if (Physics.Raycast(ray, out hit))
               {
                   //Move car
                   agent.SetDestination(hit.point);
               }
           }
   */
/*        agent.SetDestination(new Vector3(-105.5f, -0.300000012f, 0));

        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {

            //Arrived.
            Disable?.Invoke(this);
        } */
    }
}
