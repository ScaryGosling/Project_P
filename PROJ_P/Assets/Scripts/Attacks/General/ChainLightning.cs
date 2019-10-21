using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : BulletInstance
{
    private Vector3 viewportPoint;
    private Camera mainCamera;
    private float damage;
    private Collider[] hitColliders;
    private Collider bindingCollider;

    [SerializeField] float chainRadius;
    [SerializeField] float lineWidth;
    [SerializeField] Color lightningColor;
    [SerializeField] float killTime;
    [SerializeField] Material material;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            state.TakeDamage(damage);

            GetComponent<Renderer>().enabled = false;
            bindingCollider = other;


            //Find enemies within radius
            hitColliders = Physics.OverlapSphere(Player.instance.transform.position, chainRadius);
            for(int i = 0; i < hitColliders.Length; i++)
            {

                LineRenderer lr = hitColliders[i].gameObject.AddComponent<LineRenderer>();

            }

            
        }

        StartCoroutine(KillTimer());
    }


    private void Update()
    {

        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<Rigidbody>() && hitColliders[i].CompareTag("Enemy"))
            {
                LineRenderer lr = hitColliders[i].GetComponent<LineRenderer>();
                lr.material = material;
                lr.positionCount = 2;

                lr.startWidth = lineWidth;
                lr.endWidth = lineWidth;

                lr.startColor = lightningColor;
                lr.endColor = lightningColor;

                lr.SetPosition(0, hitColliders[i].transform.position);
                lr.SetPosition(1, bindingCollider.transform.position);


            }
            i++;
        }

    }


    public IEnumerator KillTimer()
    {

        yield return new WaitForSeconds(killTime);

        foreach(Collider collider in hitColliders)
        {
            Destroy(collider.gameObject.GetComponent<LineRenderer>());
        }

        Destroy(gameObject);

    }

}
