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
    protected float maginitude;
    [SerializeField] protected AudioClip impactSound;
    protected AudioSource source;
    [SerializeField] private GameObject particles;

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
            BowoniaPool.instance.AddToPool(PoolObject.WAND, gameObject);
        }
    }
    private void OnEnable()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        particles.SetActive(true);
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


    public void SetPower(float damage, float mag)
    {
        this.damage = damage;
        this.maginitude = mag;
    }

    public virtual void RunAttack(Collider other) {

        State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
        state.TakeDamage(damage, maginitude);
        if (impactSound != null && Player.instance.GetSettings().UseSFX)
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
            particles.SetActive(false) ;
            StartCoroutine(KillTimer(impactSound.length));

        }
    }

    public IEnumerator KillTimer(float time)
    {
        yield return new WaitForSeconds(time);
 
        BowoniaPool.instance.AddToPool(PoolObject.WAND, gameObject);

    }

}
