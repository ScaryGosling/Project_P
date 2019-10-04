using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    RaycastHit raycastHit;
    Ray ray;
    Vector3 lookDirection;
    Quaternion newRotation;
    [SerializeField] private float rotationSpeed = 8;

    public void Update()
    {
        SetRotation();
    }
    public void SetRotation() {


        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out raycastHit))
        {
            lookDirection = raycastHit.point - transform.position;
            lookDirection.y = 0;
            newRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }


    }



}
