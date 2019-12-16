using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Tackle")]
public class Tackle : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] float tackleForce;
    [SerializeField] float tackleLength;
    private float startSpeed;
    private float startAcceleration;
    private NavMeshAgent agent;
    [SerializeField] private List<float> cooldownPerTackleLevel = new List<float>();
    private GameObject dashParticleInstance;

    public override void RunAttack()
    {
        base.RunAttack();

        agent = player.GetComponent<NavMeshAgent>();

        dashParticleInstance = BowoniaPool.instance.GetFromPool(PoolObject.TACKLE_PARTICLE);
        dashParticleInstance.transform.SetParent(player.transform);
        dashParticleInstance.transform.localPosition = Vector3.zero;
        dashParticleInstance.transform.localEulerAngles = Vector3.zero;
        foreach (ParticleSystem ps in dashParticleInstance.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Play();
        }

        player.TackleCollider().DealDamageOnCollision = true;
        player.damage = damage;
        player.magnitude = magnitude;

        player.AnimationTrigger("Tackle");

        startAcceleration = agent.acceleration;
        startSpeed = agent.speed;

        agent.GetComponent<PlayerMovement>().enabled = false;
        agent.speed = tackleForce;
        agent.acceleration = 100;
        agent.stoppingDistance = 0.1f;
        agent.SetDestination(agent.transform.position + agent.transform.TransformDirection(Vector3.forward) * tackleLength);

        agent.GetComponent<Player>().activeStats.resistanceMultiplier = 0;

        GameObject timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
        timer.GetComponent<Timer>().RunCountDown(1, ResetStats, Timer.TimerType.DELAY);
       

    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        cooldown = cooldownPerTackleLevel[CurrentLevel];
    }

    public void DazeEnemies(GameObject[] enemies) {

        foreach (GameObject enemy in enemies) {

            enemy.GetComponent<Rigidbody>().AddForce(enemy.transform.TransformDirection(Vector3.back) * 20);
        }


    }


    public void ResetStats() {

        foreach(ParticleSystem ps in dashParticleInstance.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }
        BowoniaPool.instance.AddToPool(PoolObject.TACKLE_PARTICLE, dashParticleInstance, 1);

        agent.ResetPath();
        agent.speed = startSpeed;
        agent.acceleration = startAcceleration;
        agent.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        agent.stoppingDistance = 0;
        player.activeStats.resistanceMultiplier = 1;
        agent.GetComponent<PlayerMovement>().enabled = true;
        player.TackleCollider().DealDamageOnCollision = false;

    }
}
