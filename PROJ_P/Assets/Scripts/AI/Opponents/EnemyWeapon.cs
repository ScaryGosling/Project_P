using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{

    public AnimationClip slash;
    public Animation anim;
    public float damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Player>())
        {
            other.GetComponent<Player>().HealthProp = -damage;
        }
    }

    public void Attack()
    {
        anim.AddClip(slash, slash.name);
        anim.Play(slash.name);
    }

}
