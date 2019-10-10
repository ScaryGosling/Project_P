using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Mega Attack")]
public class MonsterAttack : PlayerAttack
{

    [SerializeField] private GameObject dodgeball;
    [SerializeField] private float force;

    public override void Execute()
    {
        base.Execute();

        Transform spawnPoint = Player.instance.GetSpawnPoint();

        GameObject ball1 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball1.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(0, 0, 1)) * force);

        GameObject ball2 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball2.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(1, 0, 1)) * force);

        GameObject ball3 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball3.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(1, 0, 0)) * force);

        GameObject ball4 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball4.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(1, 0, -1)) * force);

        GameObject ball5 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball5.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(-1, 0, 0)) * force);

        GameObject ball6 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball6.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(-1, 0, -1)) * force);

        GameObject ball7 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball7.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(0, 0, -1)) * force);

        GameObject ball8 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball8.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(-1, 0, -1)) * force);

        GameObject ball9 = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball9.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(new Vector3(-1, 0, 1)) * force);

        ball1.GetComponent<BulletInstance>().SetDamage(damage);
        ball2.GetComponent<BulletInstance>().SetDamage(damage);
        ball3.GetComponent<BulletInstance>().SetDamage(damage);
        ball4.GetComponent<BulletInstance>().SetDamage(damage);
        ball5.GetComponent<BulletInstance>().SetDamage(damage);
        ball6.GetComponent<BulletInstance>().SetDamage(damage);
        ball7.GetComponent<BulletInstance>().SetDamage(damage);
        ball8.GetComponent<BulletInstance>().SetDamage(damage);
        ball9.GetComponent<BulletInstance>().SetDamage(damage);






    }


}
