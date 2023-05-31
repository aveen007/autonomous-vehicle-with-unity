using PlasticPipe.PlasticProtocol.Messages;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using WayPointSystem;
public class WayPointManagerWindow : EditorWindow
{
    [MenuItem("Tools/Waypoint Editor")]


    public static void Open()
    {
        GetWindow<WayPointManagerWindow>();

    }

    public Transform WayPointRoot;
    private void OnGUI()
    {
        SerializedObject obj =new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("WayPointRoot"));

        if (WayPointRoot == null)
        {

            EditorGUILayout.HelpBox("Root transform must be selected", MessageType.Warning);

        }
        else
        {
            EditorGUILayout.BeginVertical("box");
            DrawButtons();
            EditorGUILayout.EndVertical();

        }

        obj.ApplyModifiedProperties();
    }
    void DrawButtons()
    {
        if (GUILayout.Button("Create Waypoint"))
        {

            CreateWaypoint();
        }

        if (Selection.activeObject != null&&Selection.activeGameObject.GetComponent<WayPoint>()){
        
            if (GUILayout.Button("Create waypoint before")) {
                CreateWayPointBefore();
            }
            if (GUILayout.Button("Create waypoint after")) {
                CreateWayPointAfter();
            }
            if (GUILayout.Button("remove waypoint")) {
                RemoveWayPoint();
            }

            if(GUILayout.Button("Add branch waypoint"))
            {
                CreateBranch();
            }
          

        }
    }


    void CreateBranch()
    {
        GameObject waypointObject = new GameObject("waypoint" + WayPointRoot.childCount, typeof(WayPoint));
        waypointObject.transform.SetParent(WayPointRoot,false);
        WayPoint wayPoint = waypointObject.GetComponent<WayPoint>();


        WayPoint branchedFrom = Selection.activeGameObject.GetComponent<WayPoint>();
        branchedFrom.branches.Add(wayPoint);

        wayPoint.transform.position = branchedFrom.transform.position;
        wayPoint.transform.forward = branchedFrom.transform.forward;


        Selection.activeGameObject=waypointObject;

    }
    void CreateWayPointBefore() {
        GameObject waypointGameObject = new GameObject("waypoint" + WayPointRoot.childCount, typeof(WayPoint));
        waypointGameObject.transform.SetParent(WayPointRoot, false);
        WayPoint waypoint= waypointGameObject.GetComponent<WayPoint>();

        WayPoint selected=Selection.activeGameObject.GetComponent<WayPoint>() as WayPoint;
        waypoint.transform.position = selected.transform.position;
        waypoint.transform.forward = selected.transform.forward;

        if (selected.PreviousWayPointl != null)
        {
            waypoint.PreviousWayPointl = selected.PreviousWayPointl;
            selected.PreviousWayPointl.NextWayPointl = waypoint;

        }
        waypoint.NextWayPointl = selected;
        selected.PreviousWayPointl= waypoint;
        waypoint.transform.SetSiblingIndex ( (selected.transform.GetSiblingIndex()));
        Selection.activeGameObject = waypointGameObject;

    }
    void CreateWayPointAfter() {
        GameObject waypointGameObject = new GameObject("waypoint" + WayPointRoot.childCount, typeof(WayPoint));
        waypointGameObject.transform.SetParent(WayPointRoot, false);
        WayPoint waypoint = waypointGameObject.GetComponent<WayPoint>();

        WayPoint selected = Selection.activeGameObject.GetComponent<WayPoint>() as WayPoint;
        waypoint.transform.position = selected.transform.position;
        waypoint.transform.forward = selected.transform.forward;

        if (selected.NextWayPointl != null)
        {
            waypoint.NextWayPointl = selected.NextWayPointl;
            selected.NextWayPointl.PreviousWayPointl = waypoint;

        }
        waypoint.PreviousWayPointl = selected;
        selected.NextWayPointl = waypoint;
        waypoint.transform.SetSiblingIndex((selected.transform.GetSiblingIndex()));
        Selection.activeGameObject = waypointGameObject;
    }
    void RemoveWayPoint() {
        WayPoint selectedWayPoint = Selection.activeGameObject.GetComponent<WayPoint>();
        if (selectedWayPoint.NextWayPointl != null)
        {
            selectedWayPoint.NextWayPointl.PreviousWayPointl = selectedWayPoint.PreviousWayPointl;

        }
        if (selectedWayPoint.PreviousWayPointl != null)
        {
            selectedWayPoint.PreviousWayPointl.NextWayPointl = selectedWayPoint.NextWayPointl;
            Selection.activeGameObject = selectedWayPoint.PreviousWayPointl.gameObject;


        }
        DestroyImmediate(selectedWayPoint.gameObject);

    }
    void CreateWaypoint()
    {
        GameObject waypointGameObject = new GameObject("waypoint" + WayPointRoot.childCount, typeof(WayPoint));
        waypointGameObject.transform.SetParent(WayPointRoot, false);

        WayPoint waypoint= waypointGameObject.GetComponent<WayPoint>();
        if (WayPointRoot.childCount > 1)
        {
            waypoint.PreviousWayPointl = WayPointRoot.GetChild(WayPointRoot.childCount - 2).GetComponent<WayPoint>();
            waypoint.PreviousWayPointl.NextWayPointl = waypoint;
            waypoint.transform.position = waypoint.PreviousWayPointl.transform.position;
            waypoint.transform.forward = waypoint.PreviousWayPointl.transform.forward;    
        }

        Selection.activeGameObject= waypointGameObject;
    }
}
