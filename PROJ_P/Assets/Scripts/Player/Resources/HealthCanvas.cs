using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    void LateUpdate()
    {
        gameObject.transform.eulerAngles = new Vector3(45,0,0);
    }
}
