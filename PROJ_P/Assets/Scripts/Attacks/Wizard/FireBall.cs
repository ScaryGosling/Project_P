using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Fireball")]
public class FireBall : PlayerAttack
{

    [SerializeField] private GameObject fireball;

    public override void RunAttack()
    {
        //base.RunAttack();
        //Instantiate(fireball, );
    }


}
