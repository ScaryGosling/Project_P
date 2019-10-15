using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesInRange.Add(other.gameObject);
            
        }
    }


    public void DamageEnemies(float damage) {

        foreach (GameObject enemy in enemiesInRange) {
            if(enemy)
                enemy.GetComponent<Unit>().TakeDamage(damage);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);

        }
    }
}
