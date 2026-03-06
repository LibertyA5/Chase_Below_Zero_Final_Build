using System.Linq.Expressions;
using UnityEngine;

public class Path : MonoBehaviour
{
    public enum PathType
    {
        Loop,
        ReverseWhenComplete
    }

    public Transform[] waypoints;
    public PathType pathType = PathType.Loop;

    private int direction = 1;
    int index;

    public Vector3 GetCurrentWayPoint()
    {
        return waypoints[index].position;
    }

    public Vector3 GetNextWayPoint()
    {
        if (waypoints.Length == 0) return transform.position;

        index = GetNextWaypointIndex();
        Vector3 nextWaypoint = waypoints[index].position;

        return nextWaypoint;
    }

    private int GetNextWaypointIndex()
    {
        // move to the next index
        index += direction;

        if (pathType == PathType.Loop)
        {
            index %= waypoints.Length;
        }
        else if (pathType == PathType.ReverseWhenComplete)
        {
            // reverse direction if at boundary
            if (index >= waypoints.Length || index < 0)
            {
                direction *= -1;
                index += direction * 2;
            }
        }

        return index;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.white;

        // draw lines between waypoints
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }

        // loop back to the start if the path is a loop
        if (pathType == PathType.Loop)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        Gizmos.color = Color.red;
        // draw the waypoints as small spheres
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.2f);
        }
    }
}
