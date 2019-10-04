using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
       
        offset = transform.position - player.transform.position;

    }
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 0.3f);
    }
}
