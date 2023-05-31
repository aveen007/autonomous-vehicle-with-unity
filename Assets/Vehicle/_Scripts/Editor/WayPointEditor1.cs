using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using WayPointSystem;

[InitializeOnLoad()]
public class WayPointEditor
{
    [DrawGizmo(GizmoType.NonSelected|GizmoType.Selected|GizmoType.Pickable)]
    public static void OnDrawSceneGizoms(WayPoint waypoint,GizmoType gizmoType)
    {
        if((gizmoType& GizmoType.Selected) != 0)
        {
            Gizmos.color = Color.yellow;

        }
        else
        {
            Gizmos.color = Color.yellow*0.5f;
        }
        Gizmos.DrawSphere(waypoint.transform.position, .1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(waypoint.transform.position + (waypoint.transform.right * waypoint.Width / 2f), waypoint.transform.position - (waypoint.transform.right * waypoint.Width / 2f));
            
        if (waypoint.PreviousWayPointl != null) {

            Gizmos.color = Color.red;
            Vector3 offset = waypoint.transform.right * waypoint.Width / 2;
            Vector3 offsetTo = waypoint.PreviousWayPointl.transform.right * waypoint.PreviousWayPointl.Width / 2;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.PreviousWayPointl.transform.position +offsetTo);

        }

        if (waypoint.NextWayPointl != null)
        {

            Gizmos.color = Color.green;
            Vector3 offset = waypoint.transform.right * -waypoint.Width / 2;
            Vector3 offsetTo = waypoint.NextWayPointl.transform.right * -waypoint.NextWayPointl.Width / 2;
            Gizmos.DrawLine(waypoint.transform.position + offset, waypoint.NextWayPointl.transform.position + offsetTo);

        }
        foreach (WayPoint branch in waypoint.branches)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(waypoint.transform.position, branch.transform.position);
        }
    }
}
