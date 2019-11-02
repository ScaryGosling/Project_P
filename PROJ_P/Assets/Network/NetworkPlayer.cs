using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{

    public Vector3 TargetPosition { get; set; }
    public Quaternion TargetRotation { get; set; }
    private float timeToReachTarget = 0.5f, time;
    private Vector3 startPosition;


    public void Update()
    {
        time += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, TargetPosition, time);
    }

    public void SetNewTarget(Vector3 targetPosition)
    {
        startPosition = transform.position;
        TargetPosition = targetPosition;
    }
}
