//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    [SerializeField] private float weight = 50f;
    [SerializeField] private float impactLevel1 = 5, impactLevel2 = 10, impactLevel3 = 15;
    private float difference;

    void Start() { Mathf.Clamp(weight, 0f, 100f); }

    /// <summary>
    /// Generic function for comparing incoming weight against unit weight. 
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    public int Compare(float weight)
    {
        if (this.weight < weight)
        {
            difference = weight - this.weight;
            return SetImpactLevel(difference);
        }
        else
            return 0;
    }


    private int SetImpactLevel(float diff)
    {
        if (diff >= impactLevel2 && diff < impactLevel3)
            return 2;
        else if (diff >= impactLevel3)
            return 3;
        else
            return 1;
    }
}
