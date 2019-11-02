using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public Vector3 TargetPosition { get; set; }
    public Quaternion TargetRotation { get; set; }
    private float timeToReachTarget = 0.5f, time;
    private Vector3 startPosition;
    private Quaternion startRotation;


    public void Update()
    {
        time += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, TargetPosition, time);
        try
        {
            transform.rotation = Quaternion.Lerp(startRotation, TargetRotation, time);
        }
        catch (Exception e) { }
        animator.SetFloat("speed", time);
    }

    public void SetNewTarget(Vector3 targetPosition, Quaternion targetRotation, float timeToNextUpdate)
    {
        timeToReachTarget = timeToNextUpdate;
        time = 0;

        startPosition = transform.position;
        TargetPosition = targetPosition;

        startRotation = transform.rotation;
        TargetRotation = targetRotation;
    }
}
