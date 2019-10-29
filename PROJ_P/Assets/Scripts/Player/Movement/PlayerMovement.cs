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
    private KeybindSet keybindSet;
    private float xMovement, zMovement;

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
        keybindSet = GetComponent<Player>().GetKeybindSet();
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

        if (Input.GetKey(keybindSet.GetBind(KeyFeature.FORWARD_MOVEMENT)))
            zMovement = 1;
        else if (Input.GetKey(keybindSet.GetBind(KeyFeature.BACKWARD_MOVEMENT)))
            zMovement = -1;
        else
            zMovement = 0;

        if (Input.GetKey(keybindSet.GetBind(KeyFeature.RIGHT_MOVEMENT)))
            xMovement = 1;
        else if (Input.GetKey(keybindSet.GetBind(KeyFeature.LEFT_MOVEMENT)))
            xMovement = -1;
        else
            xMovement = 0;


        input = new Vector3(xMovement, 0.0f, zMovement);
        rigidbody.velocity = new Vector3(0,0,0);
        agent.Move(input.normalized * Time.deltaTime * agent.speed * Player.instance.activeStats.movementSpeed);
    }

}
