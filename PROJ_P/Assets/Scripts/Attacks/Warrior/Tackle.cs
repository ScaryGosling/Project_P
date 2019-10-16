using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Tackle")]
public class Tackle : PlayerAttack
{
    [SerializeField] float tackleForce;
    [SerializeField] float tackleLength;
    private float startSpeed;
    private float startAcceleration;
    private NavMeshAgent player;

    public override void RunAttack()
    {
        base.RunAttack();

        player = Player.instance.GetComponent<NavMeshAgent>();

        startAcceleration = player.acceleration;
        startSpeed = player.speed;

        player.GetComponent<PlayerMovement>().enabled = false;
        player.speed = tackleForce;
        player.acceleration = 100;
        player.stoppingDistance = 0.1f;
        player.SetDestination(player.transform.position + player.transform.TransformDirection(Vector3.forward) * tackleLength);

        player.GetComponent<Player>().Attackable = false;

        GameObject timer = new GameObject("Timer");
        timer.AddComponent<Timer>().RunCountDown(1, ResetStats);
        


    }


    public void DazeEnemies(GameObject[] enemies) {

        foreach (GameObject enemy in enemies) {

            enemy.GetComponent<Rigidbody>().AddForce(enemy.transform.TransformDirection(Vector3.back) * 20);
        }


    }


    public void ResetStats() {

        player.ResetPath();
        player.speed = startSpeed;
        player.acceleration = startAcceleration;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        player.stoppingDistance = 0;
        player.GetComponent<Player>().Attackable = true;
        player.GetComponent<PlayerMovement>().enabled = true;

    }
}
