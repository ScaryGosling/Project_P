using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : ProjectileInstance
{

    private Collider[] hitColliders;
    private Collider bindingCollider;

    private ParticleSystem[] particlesystem;
    public float ChainRadius { get; set; }
    public float LineWidth { get; set; }
    public float Intensity { get; set; }
    public Color EmissionColor { get; set; }
    public float KillTime { get; set; }
    public Material Material { get; set; }
    public float chainEffect { get; set; }
    


    private List<Collider> enemiesInRange = new List<Collider>();
    private bool active;
    private GameObject hitParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            RunAttack(other);   
        }

        StartCoroutine(KillTimer());
    }
    private void OnEnable()
    {
        particlesystem = GetComponentsInChildren<ParticleSystem>();
        for(int i = 0; i < particlesystem.Length; i++)
        {
            particlesystem[i].Play();
        }
        GetComponent<Collider>().enabled = true;
    }

    private void CreateParticles()
    {
        hitParticles = BowoniaPool.instance.GetFromPool(PoolObject.LIGHTNING_IMPACT);
        hitParticles.transform.position = transform.position;

        if (impactSound != null && Player.instance.GetSettings().UseSFX)
        {
            source.clip = impactSound;
            source.Play();
        }
    }


    public override void RunAttack(Collider other)
    {
        State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
        state.TakeDamage(damage, maginitude);
        CreateParticles();
        Material.SetColor("_EmissionColor", EmissionColor * Intensity);

        Player.instance.gameObject.AddComponent<LineRenderer>();
        for (int i = 0; i < particlesystem.Length; i++)
        {
            particlesystem[i].Stop();
        }
        GetComponent<Collider>().enabled = false;


        Player.instance.AnimationTrigger("Lightning");

        bindingCollider = other;
        bindingCollider.gameObject.AddComponent<LineRenderer>();

        //Find enemies within radius
        hitColliders = Physics.OverlapSphere(transform.position, ChainRadius);
        FindEnemies(hitColliders);
        hitColliders = null;


        foreach (Collider collider in enemiesInRange)
        {
            if(!collider.gameObject.GetComponent<LineRenderer>())
                collider.gameObject.AddComponent<LineRenderer>();

            if (collider.GetComponent<Unit>())
                collider.GetComponent<Unit>().currentState.TakeDamage(damage * chainEffect, maginitude);

        }
        active = true;
    }


    public int FindEnemies(Collider[] colliders)
    {
        int i = 0;
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                enemiesInRange.Add(collider);
                i++;
            }

        }

            return i;

        
    }



    private void Update()
    {
        if (enemiesInRange == null || !active)
            return;

            if (Player.instance.gameObject.GetComponent<LineRenderer>())
            {
                LineRenderer initialLine = Player.instance.gameObject.GetComponent<LineRenderer>();
                initialLine.material = Material;
                initialLine.positionCount = 2;

                initialLine.startWidth = LineWidth;
                initialLine.endWidth = LineWidth;

                initialLine.SetPosition(0, Player.instance.transform.position);
                initialLine.SetPosition(1, bindingCollider.transform.position);

            }

            if (bindingCollider.GetComponent<LineRenderer>() && enemiesInRange.Count > 0)
            {
                LineRenderer bind = bindingCollider.GetComponent<LineRenderer>();
                bind.material = Material;
                bind.positionCount = 2;

                bind.startWidth = LineWidth;
                bind.endWidth = LineWidth;

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
                        lr.material = Material;
                        lr.positionCount = 2;

                        lr.startWidth = LineWidth;
                        lr.endWidth = LineWidth;

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

        yield return new WaitForSeconds(KillTime);
        ClearColliders();
        enemiesInRange.Clear();
        BowoniaPool.instance.AddToPool(PoolObject.LIGHTNING_IMPACT, hitParticles);
        BowoniaPool.instance.AddToPool(PoolObject.LIGHTNING, gameObject);

    }

}
