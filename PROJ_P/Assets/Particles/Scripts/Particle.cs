using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float time;
    public IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void Start()
    {
        StartCoroutine(KillTimer());
    }
}
