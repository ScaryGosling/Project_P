using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{

  
    [SerializeField] private float rotationSpeed = 8;
    private Camera mainCamera;
    private NavMeshAgent agent;
    private Rigidbody rigid;
    private KeybindSet keybindSet;
    private float xMovement, zMovement;
    [SerializeField]private Animator animator;
    [SerializeField] private LayerMask ignoreMask;
    private float raycastDistance = 100;
    private Player player;

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
        rigid = GetComponent<Rigidbody>();
        keybindSet = GetComponent<Player>().GetKeybindSet();
        player = GetComponent<Player>();
    }

    RaycastHit hit;
    Ray ray;
    Quaternion newRotation;
    Vector3 lookDirection;

    private void SetRotation()
    {
        
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, raycastDistance, ~ignoreMask))
        {
            lookDirection = hit.point - transform.position;
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
        rigid.velocity = new Vector3(0,0,0);
        agent.Move(input.normalized * Time.deltaTime * agent.speed * Player.instance.activeStats.movementSpeed);

        animator.SetFloat("speed", Vector3.Dot(transform.forward, input.normalized));
        animator.SetFloat("direction", Vector3.Dot(transform.right, input.normalized));


    }

}
