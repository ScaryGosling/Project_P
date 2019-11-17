//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Boomer/BoomerDeath")]
public class BoomerDeath : DeathBase
{
    [SerializeField] private float scaleFactor = 1.00001f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip fuseClip;
    private Explosion explosion;

    private const float animationTime = 1f;
    [SerializeField] private float explosionDelay = 2f;
    private float currentTimeAnimTime;
    private float current;
    private bool completed;
    public override void EnterState()
    {
        base.EnterState();
        current = explosionDelay;
        Explode();
        //WarningObject.SetActive(true);
    }

    public override void ToDo()
    {
        base.ToDo();
    }

    protected override void DeathAnimation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        owner.AliveProp = false;
        currentTimeAnimTime = animationTime;
        while (currentTimeAnimTime >= 0f)
        {
            owner.transform.localScale *= scaleFactor;
            currentTimeAnimTime -= Time.deltaTime;
        }
    }

    protected void Explode()
    {
        explosion = Instantiate(explosionPrefab, owner.transform.position, Quaternion.Euler(-90f, 0f, 0f)).GetComponent<Explosion>();
        explosion.SetupSounds(fuseClip, explosionClip);
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