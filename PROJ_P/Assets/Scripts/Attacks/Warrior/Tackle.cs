using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Tackle")]
public class Tackle : PlayerAttack
{
    [SerializeField] float tackleForce;

    public override void RunAttack()
    {
        base.RunAttack();
        GameObject player = Player.instance.gameObject;
        
        player.GetComponent<Rigidbody>().AddForce(tackleForce * player.transform.TransformDirection(Vector3.forward));
    }
}
