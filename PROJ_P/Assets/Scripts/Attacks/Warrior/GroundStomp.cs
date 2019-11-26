using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Ground Stomp")]
public class GroundStomp : PlayerAttack
{
    [Header("Ability Specific")]
    private float attackRadius;
    [SerializeField] private float stompForce;
    [SerializeField] private float dazeTime;
    [SerializeField] private List<float> radiusPerLevel = new List<float>();
    private GameObject addedParticle;

    public override void RunAttack()
    {
        base.RunAttack();

        //Particle system
        addedParticle = Instantiate(particles, Player.instance.transform.position - new Vector3(0, Player.instance.transform.lossyScale.y /2, 0), Quaternion.identity);
        addedParticle.transform.GetChild(0).localScale = new Vector3(attackRadius, 1, attackRadius);

        //Find enemies in circle around player
        Collider[] hitColliders = Physics.OverlapSphere(Player.instance.transform.position, attackRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<Rigidbody>() && !hitColliders[i].CompareTag("Player")) {

                hitColliders[i].GetComponent<NavMeshAgent>().enabled = false;
                hitColliders[i].GetComponent<Rigidbody>().AddForce(hitColliders[i].transform.TransformDirection(Vector3.back) * stompForce);
                hitColliders[i].GetComponent<Unit>().currentState.TakeDamage(damage, magnitude);

            }
            i++;
        }

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(dazeTime, ResetEnemies, Timer.TimerType.DELAY);
    }

    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        attackRadius = radiusPerLevel[CurrentLevel];
    }

    public void ResetEnemies()
    {
        Destroy(addedParticle);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.GetComponent<NavMeshAgent>().enabled = true;
        }
    }


}
