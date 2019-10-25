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
                other.GetComponent<NavMeshAgent>().isStopped = true;
            else
                other.GetComponent<NavMeshAgent>().isStopped = false;
        }
    }



    public IEnumerator FreezeTime() {

        while (Timer > 0) {

            yield return null;
            Timer -= Time.deltaTime;

        }

        active = false;
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);


    }

}
