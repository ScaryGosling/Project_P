using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class FireballInstance : MonoBehaviour
{
    public float ExplosionRadius { get; set; }
    public float Damage { get; set; }
    public float Magnitude { get; set; }

    private AudioSource source;
    [SerializeField] private AudioClip impactSound;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        if (impactSound != null)
            source.clip = impactSound;
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        Explode(other);
    }

    public void Explode(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;

        if(impactSound != null && Player.instance.GetSettings().UseSFX)
            source.Play();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach(Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                col.GetComponent<Unit>().currentState.TakeDamage(Damage, Magnitude);
            }
        }
        StartCoroutine(KillTimer(impactSound.length));
    }

    ////Remove this and add real animation instead
    //public IEnumerator ExplosionAnimation() {

    //    while (transform.lossyScale.x < ExplosionRadius) {

    //        transform.localScale += new Vector3(1, 1, 1);
    //        yield return null;
    //    }
    //    StartCoroutine(KillTimer(impactSound.length));

    //}

    public IEnumerator KillTimer(float time)
    {
        yield return new WaitForSeconds(time);
        BowoniaPool.instance.AddToPool(PoolObject.FIREBALL, gameObject);

    }

}
