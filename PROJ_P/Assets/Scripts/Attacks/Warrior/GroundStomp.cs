﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Ground Stomp")]
public class GroundStomp : PlayerAttack
{

    [SerializeField] private float attackRadius;
    [SerializeField] private float stompForce;

    public override void RunAttack()
    {
        base.RunAttack();

        //Find enemies in circle around player
        Collider[] hitColliders = Physics.OverlapSphere(Player.instance.transform.position, attackRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].GetComponent<Rigidbody>() && !hitColliders[i].CompareTag("Player")) {

                hitColliders[i].GetComponent<NavMeshAgent>().enabled = false;
                hitColliders[i].GetComponent<Rigidbody>().AddForce(hitColliders[i].transform.TransformDirection(Vector3.back) * stompForce);

            }
            i++;
        }


    }


}
