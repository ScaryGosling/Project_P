using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileInstance : MonoBehaviour
{
    private Vector3 viewportPoint;
    private Camera mainCamera;
    protected float damage;
    [SerializeField] protected AudioClip impactSound;
    protected AudioSource source;

    private void Start()
    {
        mainCamera = Camera.main;
        source = GetComponent<AudioSource>();
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
        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1 || transform.position.y < 0)
        {
            return false;
        }
        return true;
    }


    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public virtual void RunAttack(Collider other) {

        State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
        state.TakeDamage(damage);
        if (impactSound != null)
        {
            source.clip = impactSound;
            source.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
           
            RunAttack(other);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(KillTimer(impactSound.length));

        }
    }

    public IEnumerator KillTimer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);

    }

}
