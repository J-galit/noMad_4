using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringUtility : MonoBehaviour
{
    public static Vector3 Seek(Vector3 origin, Vector3 target)
    {
        Vector3 direction = target - origin;
        return direction.normalized;
    }

    public static Vector3 Seek(Transform origin, Transform target)
        => Seek(origin.position, target.position);

    public static Vector3 Flee(Vector3 origin, Vector3 target)
        => Seek(target, origin);

    public static Vector3 Flee(Transform origin, Transform target)
        => Seek(target.position, origin.position);
}
