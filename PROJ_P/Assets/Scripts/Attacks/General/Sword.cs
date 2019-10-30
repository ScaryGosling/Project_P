using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private float damage;
    private PlayerAttack hack;
    [SerializeField] private GameObject bloodSplatter;
    private GameObject addedSplatter;
    public void CacheComponents(float damage, PlayerAttack hack)
    {
        this.damage = damage;
        this.hack = hack;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Player.instance.Resource.DrainResource(hack);
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            if (state)
            {
                state.TakeDamage(damage);
                addedSplatter = Instantiate(bloodSplatter, other.transform.position, Quaternion.identity);
                Timer timer = addedSplatter.AddComponent<Timer>();
                timer.RunCountDown(4, PlaceboMethod);
            }

        }
    }

    public void PlaceboMethod() { }


}
