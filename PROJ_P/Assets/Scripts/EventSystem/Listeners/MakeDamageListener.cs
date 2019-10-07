using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamageListener : MonoBehaviour
{
    void Start()
    {
        EventSystem.Current.RegisterListener<MakeDamageEvent>(MakeDamage);
    }

    void MakeDamage(MakeDamageEvent makeDamage)
    {
        Debug.Log(makeDamage.Damage + " damage applied");
    }
}
