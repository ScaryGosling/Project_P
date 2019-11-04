using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Freeze : MonoBehaviour
{
    public float Timer { get; set; }
    private List<NavMeshAgent> stoppedAgents = new List<NavMeshAgent>();



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;
            stoppedAgents.Add(other.GetComponent<NavMeshAgent>());

        }
    }



    public IEnumerator FreezeTime() {

        while (Timer > 0) {

            yield return null;
            Timer -= Time.deltaTime;

        }

        stoppedAgents.ForEach(agent => agent.isStopped = false);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);


    }

}
