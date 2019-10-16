using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 8;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigidbody;

    public void Update()
    {
        SetRotation();
        SetMovement();
    }

    public void Start()
    {
        CacheComponents();
    }

    public void CacheComponents() {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    RaycastHit raycastHit;
    Ray ray;
    Quaternion newRotation;
    Vector3 lookDirection;

    private void SetRotation()
    {
        
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit))
        {
            lookDirection = raycastHit.point - transform.position;
            lookDirection.y = 0;
            newRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }


    }


    Vector3 input;
    private void SetMovement()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        rigidbody.velocity = new Vector3(0,0,0);
        agent.Move(input.normalized * Time.deltaTime * agent.speed);
    }

}
