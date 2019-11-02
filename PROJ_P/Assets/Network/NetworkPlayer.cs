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
    private Vector3 lastFramePosition;


    public void Update()
    {
        time += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, TargetPosition, time);

        transform.eulerAngles = Vector3.Lerp(startRotation.eulerAngles, TargetRotation.eulerAngles, time);

        Vector3 input = (transform.position - lastFramePosition) / Time.deltaTime;
        animator.SetFloat("speed", Vector3.Dot(transform.forward, input.normalized));
        animator.SetFloat("direction", Vector3.Dot(transform.right, input.normalized));
        lastFramePosition = transform.position;
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
