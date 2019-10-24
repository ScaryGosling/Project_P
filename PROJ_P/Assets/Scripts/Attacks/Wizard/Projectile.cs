using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Projectile")]
public class Projectile : PlayerAttack
{

    [SerializeField] private GameObject projectile;
    [SerializeField] private float force;

    public override void Execute()
    {
        base.Execute();

    }
    

    public override void RunAttack()
    {
        base.RunAttack();
        Transform spawnPoint = Player.instance.GetSpawnPoint();

        GameObject ball = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        ball.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(Vector3.forward) * force);
        ball.GetComponent<BulletInstance>().SetDamage(damage);
    }

}
