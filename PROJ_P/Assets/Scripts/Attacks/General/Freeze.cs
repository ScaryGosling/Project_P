using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Freeze : MonoBehaviour
{
    public float Timer { get; set; }
    public float Damage { get; set; }
    public float Magnitude { get; set; }

    private List<NavMeshAgent> stoppedAgents = new List<NavMeshAgent>();



    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && Timer >0)
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;
            other.GetComponent<Unit>().currentState.TakeDamage(Damage, Magnitude);
            stoppedAgents.Add(other.GetComponent<NavMeshAgent>());
        }
    }



    public IEnumerator FreezeTime() {

        while (Timer > 0) {

            yield return null;
            Timer -= Time.deltaTime;
        }


        foreach(NavMeshAgent agent in stoppedAgents)
        {
            if(agent != null)
            {
                agent.isStopped = false;
            }
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);


    }

}
