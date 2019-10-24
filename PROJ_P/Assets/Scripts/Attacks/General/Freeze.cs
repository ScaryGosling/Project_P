using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Freeze : MonoBehaviour
{
    public float Timer { get; set; }
    private bool active = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy")) {

            if (active)
                other.GetComponent<NavMeshAgent>().enabled = false;
            else
                other.GetComponent<NavMeshAgent>().enabled = true;
        }
    }



    public IEnumerator FreezeTime() {

        while (Timer > 0) {

            yield return null;
            Timer -= Time.deltaTime;

        }

        active = false;
        yield return null;
        Destroy(gameObject);


    }

}
