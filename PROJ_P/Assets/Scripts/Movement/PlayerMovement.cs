using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    RaycastHit raycastHit;
    Ray ray;
    Vector3 newPos;
    Quaternion rotation;
    [SerializeField] private float rotationSpeed = 8;

    public void Update()
    {
        SetRotation();
    }
    public void SetRotation() {


        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out raycastHit))
        {
            newPos = raycastHit.point - transform.position;
            newPos.y = 0;
            rotation = Quaternion.LookRotation(newPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }


    }



}
