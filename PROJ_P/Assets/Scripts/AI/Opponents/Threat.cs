//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Threat : MonoBehaviour
{
    private float threat, time, amount;
    [SerializeField] private float threatThreshold = 20f;
    public float thresholdProp { get { return threatThreshold; } }
    public float threatProp { get { return threat; } set { threat = value; } }
    private Unit unit;
    

    void Start()
    {
        unit = gameObject.GetComponent<Unit>();
    }

    private void OnEnable()
    {
        ResetThreat();
    }

    public void PeekThreat(float peekAmount)
    {
        if (threat < 100f)
        {
            threatProp += peekAmount;
            Mathf.Clamp(threatProp, 0f, 100f);
        }
    }

    public void StartThreatGeneration(float time, float amount)
    {
        this.time = time;
        this.amount = amount;

        StartCoroutine(ThreatGenerator());
    }

    public void HaltThreatGeneration()
    {
        ResetThreat();
        StopCoroutine(ThreatGenerator());
    }

    public void ResetThreat()
    {
        threat = 0f;
    }

    IEnumerator ThreatGenerator()
    {
        while (time < 0)
        {
            Debug.Log("Curr - Time: " + time);
            if (threat < 100f)
            {
                threatProp += amount;
                Mathf.Clamp(threatProp, 0f, 100f);
            }
            time -= Time.deltaTime;
            yield return new WaitForSeconds(0.5f);
        }

        HaltThreatGeneration();
    }

}
