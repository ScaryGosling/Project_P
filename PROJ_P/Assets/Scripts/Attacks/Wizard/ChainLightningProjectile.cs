using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/ChainLightning")]
public class ChainLightningProjectile : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float force;

    [Header("Lightning Elements")]
    [SerializeField] private float chainRadius;
    [Tooltip("Amount of damage caused to chained enemies in percent. 100% means full damage.")]
    [SerializeField] [Range(0,100)] private float chainEffect = 20;
    [SerializeField] private float lineWidth;
    [SerializeField] private Color emissionColor;
    [SerializeField] private float intensity;
    [SerializeField] private float killTime;
    [SerializeField] private Material material;


    public override void RunAttack()
    {
        base.RunAttack();
        
        Transform spawnPoint = Player.instance.GetSpawnPoint();

        GameObject ball = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

        ChainLightning ch = ball.GetComponent<ChainLightning>();
        ch.ChainRadius = chainRadius;
        ch.LineWidth = lineWidth;
        ch.Intensity = intensity;
        ch.KillTime = killTime;
        ch.Material = material;
        ch.EmissionColor = emissionColor;
        ch.chainEffect = chainEffect/100;

        ball.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(Vector3.forward) * force);
        ball.GetComponent<ProjectileInstance>().SetPower(damage, magnitude);

    }
}
