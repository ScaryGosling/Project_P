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
    private float chainRadius;
    [Tooltip("Amount of damage caused to chained enemies in percent. 100% means full damage.")]
    [SerializeField] [Range(0,100)] private float chainEffect = 20;
    [SerializeField] private float lineWidth;
    [SerializeField] private Color emissionColor;
    [SerializeField] private float intensity;
    [SerializeField] private float killTime;
    [SerializeField] private Material material;
    [SerializeField] private List<float> radiusPerLevel = new List<float>();



    public override void RunAttack()
    {
        base.RunAttack();
        Transform spawnPoint = player.GetSpawnPoint();
        GameObject ball = GetProjectile();

        ChainLightning ch = ball.GetComponent<ChainLightning>();
        ch.ChainRadius = chainRadius;
        ch.LineWidth = lineWidth;
        ch.Intensity = intensity;
        ch.KillTime = killTime;
        ch.Material = material;
        ch.EmissionColor = emissionColor;
        ch.chainEffect = chainEffect/100;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().AddForce(spawnPoint.TransformDirection(Vector3.forward) * force);
        ball.GetComponent<ProjectileInstance>().SetPower(damage, magnitude);
    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        chainRadius = radiusPerLevel[CurrentLevel];
    }
    private GameObject GetProjectile()
    {
        Transform spawnPoint = player.GetSpawnPoint();
        GameObject ball = BowoniaPool.instance.GetFromPool(PoolObject.LIGHTNING);
        ball.transform.position = spawnPoint.position;
        ball.transform.rotation = spawnPoint.rotation;
        return ball;
    }
}
