using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private bool running = false;
    public bool isRunning { get { return running;  } set { running = value; } }
    [SerializeField] private GameObject unitPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            StartCoroutine(unitSpawner());
        }
    }

    IEnumerator unitSpawner()
    {
        Instantiate(unitPrefab, gameObject.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2);
    }
}
