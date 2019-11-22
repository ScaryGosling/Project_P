using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCanvas : MonoBehaviour
{
    void LateUpdate()
    {
        gameObject.transform.eulerAngles = Vector3.zero;
    }
}
