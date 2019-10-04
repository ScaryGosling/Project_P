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
        offset.x = transform.position.x - player.transform.position.x;
        offset.y = transform.position.y - player.transform.position.y;
        offset.z = transform.position.z - player.transform.position.z;
    }
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + offset, ref velocity, 0.3f);
    }
}
