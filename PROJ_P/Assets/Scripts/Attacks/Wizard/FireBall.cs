using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Wizard/Fireball")]
public class FireBall : PlayerAttack
{

    [SerializeField] private GameObject fireball;
    [SerializeField] private float forwardForce, upForce;
    private Transform spawnPoint;


    public override void RunAttack()
    {
        base.RunAttack();
        Rigidbody rb = Instantiate(fireball, spawnPoint.position + spawnPoint.TransformDirection(Vector3.forward) * 2, spawnPoint.rotation).GetComponent<Rigidbody>();
        rb.AddForce(rb.transform.TransformDirection(Vector3.forward) * forwardForce);
    }

    public override void OnEquip()
    {
        spawnPoint = Player.instance.GetSpawnPoint();
    }
}
