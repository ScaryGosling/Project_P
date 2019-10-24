using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Fireball")]
public class FireBall : PlayerAttack
{

    [SerializeField] private GameObject fireball;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float explosionRadius;
    private Transform spawnPoint;


    public override void RunAttack()
    {
        base.RunAttack();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 50))
        {
            FireballInstance instance = Instantiate(fireball, hit.point + new Vector3(0, spawnHeight, 0), Quaternion.identity).GetComponent<FireballInstance>();
            instance.ExplosionRadius = explosionRadius;
            instance.Damage = damage;
            instance.GetComponent<Rigidbody>().AddForce(Vector3.down * 1000);
            
        }
    
        
    }

    public override void OnEquip()
    {
    }
}
