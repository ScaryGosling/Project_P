//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/BoomerDeath")]
public class BoomerDeath : DeathBase
{
    [SerializeField] private float scaleFactor = 1.00001f;
    [SerializeField] private GameObject explosionPrefab;
    private GameObject explosion;

    private const float animationTime = 1.5f;
    private const float explosionDelay = 0.5f;
    private float currentTime;

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
    }

    protected override void DeathRotation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        alive = false;

        currentTime = animationTime;
        while (currentTime >= 0f)
        {
            owner.transform.localScale *= scaleFactor;
            currentTime -= Time.deltaTime;
        }

        if (currentTime <= animationTime / 0 && explosion == null)
            Explode();
    }

    private void Explode()
    {
        explosion = Instantiate(explosionPrefab, owner.transform.position, Quaternion.identity);
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