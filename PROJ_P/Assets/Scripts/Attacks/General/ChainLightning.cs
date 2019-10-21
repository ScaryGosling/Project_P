using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : BulletInstance
{

    private Collider[] hitColliders;
    private Collider bindingCollider;
    private bool active;

    [SerializeField] float chainRadius;
    [SerializeField] float lineWidth;
    [SerializeField] Color lightningColor;
    [SerializeField] float killTime;
    [SerializeField] Material material;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            RunAttack(other);   
        }

        StartCoroutine(KillTimer());
    }

    public override void RunAttack(Collider other)
    {
        base.RunAttack(other);

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        bindingCollider = other;

        //Find enemies within radius
        hitColliders = Physics.OverlapSphere(Player.instance.transform.position, chainRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {

            hitColliders[i].gameObject.AddComponent<LineRenderer>();

            if(hitColliders[i].GetComponent<Unit>())
                hitColliders[i].GetComponent<Unit>().currentState.TakeDamage(damage / 5);
        }
    }

    private void Update()
    {
        if (active && hitColliders != null) {
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].GetComponent<Rigidbody>() && hitColliders[i].CompareTag("Enemy") && hitColliders[i].GetComponent<LineRenderer>())
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

    }

    public void ClearColliders()
    {
        LineRenderer[] linesToRemove = FindObjectsOfType<LineRenderer>();

        foreach (LineRenderer line in linesToRemove)
        {
            Destroy(line);
        }

    }


    public IEnumerator KillTimer()
    {
        active = true;
        yield return new WaitForSeconds(killTime);
        active = false;
        ClearColliders();

        Destroy(gameObject);

    }

}
