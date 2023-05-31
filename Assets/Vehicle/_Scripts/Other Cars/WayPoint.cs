using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WayPointSystem
{
    public class WayPoint : MonoBehaviour
    {
        public WayPoint PreviousWayPointl;
        public WayPoint NextWayPointl;

        [Range(0f, 15f)]
        public float Width = 10f;
        [Range(0f, 1f)]
        public float branchRatio=0.5f;
        public List<WayPoint> branches = new List<WayPoint>();
        public Vector3 GetPosition()
        {
            Vector3 minBound = transform.position + transform.right * Width / 2f;
            Vector3 maxBound = transform.position - transform.right * Width / 2f;
            return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));


        }
    }
}