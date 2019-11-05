using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveCounter : MonoBehaviour
{
    [SerializeField] private float countdown = 3;
    Animator animator;
    [SerializeField] private TMP_Text text;
    private int waveNumber;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void TriggerUpdateOfCounter()
    {

        text.text = waveNumber.ToString();
    }
    public void UpdateCounter(int newNumber)
    {
        waveNumber = newNumber;
    }

    public void StartNewWave()
    {
        animator.SetTrigger("StartNewWave");
    }
    public void CountdownBeforeRotation()
    {

        StartCoroutine(CountDown());
    }
    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(countdown);
        animator.SetTrigger("CountdownDone");
    }
}
