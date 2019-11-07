using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionQuestBuildingHittingPoints : MonoBehaviour
{
    [SerializeField] private GameObject[] points;

    public GameObject GetRandomPoint()
    {
        return points[Random.Range(0, points.Length)];
    }
}
