using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : BulletInstance
{

    private Collider[] hitColliders;
    private Collider bindingCollider;

    [SerializeField] float chainRadius;
    [SerializeField] float lineWidth;
    [SerializeField] Color lightningColor;
    [SerializeField] float killTime;
    [SerializeField] Material material;


    private List<Collider> enemiesInRange = new List<Collider>();
    private bool active;

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

        Player.instance.gameObject.AddComponent<LineRenderer>();

        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        bindingCollider = other;
        bindingCollider.gameObject.AddComponent<LineRenderer>();

        //Find enemies within radius
        hitColliders = Physics.OverlapSphere(Player.instance.transform.position, chainRadius);
        FindEnemies(hitColliders);
        hitColliders = null;


        foreach (Collider collider in enemiesInRange)
        {
            if(!collider.gameObject.GetComponent<LineRenderer>())
                collider.gameObject.AddComponent<LineRenderer>();

            if (collider.GetComponent<Unit>())
                collider.GetComponent<Unit>().currentState.TakeDamage(damage / 5);

        }
        active = true;
    }


    public void FindEnemies(Collider[] colliders)
    {
    
        foreach(Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
                enemiesInRange.Add(collider);

        }


    }



    private void Update()
    {
        if (enemiesInRange == null || !active)
            return;

            if (Player.instance.gameObject.GetComponent<LineRenderer>())
            {
                LineRenderer initialLine = Player.instance.gameObject.GetComponent<LineRenderer>();
                initialLine.material = material;
                initialLine.positionCount = 2;

                initialLine.startWidth = lineWidth;
                initialLine.endWidth = lineWidth;

                initialLine.startColor = lightningColor;
                initialLine.endColor = lightningColor;

                initialLine.SetPosition(0, Player.instance.transform.position);
                initialLine.SetPosition(1, bindingCollider.transform.position);

            }

            if (bindingCollider.GetComponent<LineRenderer>() && enemiesInRange.Count > 0)
            {
                LineRenderer bind = bindingCollider.GetComponent<LineRenderer>();
                bind.material = material;
                bind.positionCount = 2;

                bind.startWidth = lineWidth;
                bind.endWidth = lineWidth;

                bind.startColor = lightningColor;
                bind.endColor = lightningColor;

                bind.SetPosition(0, bindingCollider.transform.position);
                bind.SetPosition(1, enemiesInRange[0].transform.position);
            }
            

            if (enemiesInRange.Count > 1)
            {

                for (int i = 1; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i] && enemiesInRange[i].GetComponent<LineRenderer>())
                    {

                        LineRenderer lr = enemiesInRange[i].GetComponent<LineRenderer>();
                        lr.material = material;
                        lr.positionCount = 2;

                        lr.startWidth = lineWidth;
                        lr.endWidth = lineWidth;

                        lr.startColor = lightningColor;
                        lr.endColor = lightningColor;

                        lr.SetPosition(0, enemiesInRange[i - 1].transform.position);
                        lr.SetPosition(1, enemiesInRange[i].transform.position);

                    }
                    
                }



            }
            




    }

    public void ClearColliders()
    {
        LineRenderer[] linesToRemove = FindObjectsOfType<LineRenderer>();

        Destroy(Player.instance.GetComponent<LineRenderer>());

        foreach (LineRenderer line in linesToRemove)
        {
            Destroy(line);
        }

    }


    public IEnumerator KillTimer()
    {

        yield return new WaitForSeconds(killTime);
        ClearColliders();

        Destroy(gameObject);

    }

}
