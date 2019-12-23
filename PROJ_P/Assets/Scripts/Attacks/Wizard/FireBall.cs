using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Fireball")]
public class FireBall : PlayerAttack
{

    [Header("Ability Specific")]
    [SerializeField] private GameObject fireball;
    [SerializeField] private float spawnHeight;
    private float explosionRadius;
    private Transform spawnPoint;
    [SerializeField] private List<float> radiusPerLevel = new List<float>();


    protected override void SetTooltipText()
    {
        tooltip =            "Damage: " + upgradeCosts[CurrentLevel].newDamage + "->" +
            upgradeCosts[CurrentLevel + 1].newDamage.ToString() + "\n" +
            "Area of Effect: " + radiusPerLevel[CurrentLevel].ToString() + "->" +
            radiusPerLevel[CurrentLevel + 1].ToString();
    }

    public override void RunAttack()
    {
        base.RunAttack();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 50))
        {
            FireballInstance instance = GetFireball(hit).GetComponent<FireballInstance>();
            instance.ExplosionRadius = explosionRadius;
            instance.Damage = damage;
            instance.Magnitude = magnitude;
            instance.GetComponent<Rigidbody>().AddForce(Vector3.down * 1000);
            player.AnimationTrigger("Fireball");
        }
    }

    private GameObject GetFireball(RaycastHit hit)
    {   
        GameObject ball = BowoniaPool.instance.GetFromPool(PoolObject.FIREBALL);
        ball.transform.position = hit.point + new Vector3(0, spawnHeight, 0);
        ball.transform.rotation = Quaternion.identity;
        return ball;
    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        explosionRadius = radiusPerLevel[CurrentLevel];
    }
    public override void OnEquip()
    {
    }
}
