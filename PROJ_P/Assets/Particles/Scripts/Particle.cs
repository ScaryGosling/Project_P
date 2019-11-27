using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public float disableTime;
    [SerializeField] private PoolObject type;
    public IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(disableTime);
        BowoniaPool.instance.AddToPool(type, gameObject);
    }

    public void OnEnable()
    {
        StartCoroutine(KillTimer());
    }
}
