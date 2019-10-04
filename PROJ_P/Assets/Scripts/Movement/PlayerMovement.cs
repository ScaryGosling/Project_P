using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    RaycastHit raycastHit;
    Ray ray;
    Vector3 lookDirection, input;
    Quaternion newRotation;
    [SerializeField] private float rotationSpeed = 8;

    public void Update()
    {
        SetRotation();
        SetMovement();
    }
    private void SetRotation() {


        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out raycastHit))
        {
            lookDirection = raycastHit.point - transform.position;
            lookDirection.y = 0;
            newRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }


    }

    private  void SetMovement()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        transform.position += input * Time.deltaTime * 8;
    }

}
