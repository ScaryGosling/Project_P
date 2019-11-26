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
    private NavMeshAgent player;
    [SerializeField] private List<float> cooldownPerTackleLevel = new List<float>();

    public override void RunAttack()
    {
        base.RunAttack();

        player = Player.instance.GetComponent<NavMeshAgent>();
        Player playerComp = player.GetComponent<Player>();

        playerComp.dealDamageOnCollision = true;
        playerComp.damage = damage;
        playerComp.magnitude = magnitude;

        startAcceleration = player.acceleration;
        startSpeed = player.speed;

        player.GetComponent<PlayerMovement>().enabled = false;
        player.speed = tackleForce;
        player.acceleration = 100;
        player.stoppingDistance = 0.1f;
        player.SetDestination(player.transform.position + player.transform.TransformDirection(Vector3.forward) * tackleLength);

        player.GetComponent<Player>().activeStats.resistanceMultiplier = 0;

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

        Player playerComp = player.GetComponent<Player>();

        player.ResetPath();
        player.speed = startSpeed;
        player.acceleration = startAcceleration;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        player.stoppingDistance = 0;
        playerComp.activeStats.resistanceMultiplier = 1;
        player.GetComponent<PlayerMovement>().enabled = true;
        playerComp.dealDamageOnCollision = false;

    }
}
