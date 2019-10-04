using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public void Update()
    {
        SetRotation();
    }

    public void SetRotation() {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point);
            transform.eulerAngles = transform.eulerAngles
        }


    }



}
