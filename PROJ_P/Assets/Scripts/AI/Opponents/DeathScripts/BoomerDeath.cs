//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/BoomerDeath")]
public class BoomerDeath : DeathBase
{
    [SerializeField] private float scaleFactor = 1.00001f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject WarningObject;
    private GameObject explosion;

    private const float animationTime = 1f;
    [SerializeField] private float explosionDelay = 2f;
    private float currentTimeAnimTime;
    private float current;
    private bool completed;
    public override void EnterState()
    {
        base.EnterState();
        current = explosionDelay;
        //WarningObject.SetActive(true);
    }

    public override void ToDo()
    {
        base.ToDo();
        Explode();
    }

    protected override void DeathAnimation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        alive = false;

        currentTimeAnimTime = animationTime;
        while (currentTimeAnimTime >= 0f)
        {
            owner.transform.localScale *= scaleFactor;
            currentTimeAnimTime -= Time.deltaTime;
        }
    }

    protected void Explode()
    {
        if (current >= 0f)
        {
            current -= Time.deltaTime;
        }
        else if (current <= 0f && explosion == null)
        {
            explosion = Instantiate(explosionPrefab, owner.transform.position, Quaternion.identity);
            //WarningObject.SetActive(false);
        }
    }
}

#region ChaseLegacy
// lightAngle = lightField.spotAngle;
//ChaseEvent chaseEvent = new ChaseEvent();
//chaseEvent.gameObject = owner.gameObject;
//chaseEvent.eventDescription = "Chasing Enemy";
//chaseEvent.audioSpeaker = audioSpeaker;

//EventSystem.Current.FireEvent(chaseEvent);
#endregion