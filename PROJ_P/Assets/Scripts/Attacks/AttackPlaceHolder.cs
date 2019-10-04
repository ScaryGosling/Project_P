using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/DodgeballAttack")]
public class AttackPlaceHolder : PlayerAttack
{

    [SerializeField] private GameObject dodgeball;
    [SerializeField] private float force;

    public override void Execute(Transform spawnPoint)
    {

        GameObject ball = Instantiate(dodgeball, spawnPoint.position, spawnPoint.rotation);
        ball.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(Vector3.forward) * force);


    }


}
