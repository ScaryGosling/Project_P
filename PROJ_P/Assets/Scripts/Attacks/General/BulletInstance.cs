using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletInstance : MonoBehaviour
{
    private Vector3 viewportPoint;
    private Camera mainCamera;
    private float damage;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        CheckForBulletRemoval();
    }

    private void CheckForBulletRemoval()
    {
        if (!IsVisible())
        {
            Destroy(gameObject);
        }
    }

    private bool IsVisible()
    {
        viewportPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            return false;
        }
        return true;
    }
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //other.GetComponent<Unit>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
