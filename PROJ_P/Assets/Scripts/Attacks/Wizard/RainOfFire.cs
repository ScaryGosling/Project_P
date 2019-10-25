using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Rain of Fire")]
public class RainOfFire : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private int spawnAmount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float spawnHeight;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float force;

    private Vector3 point;

    public override void RunAttack()
    {
        base.RunAttack();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 50))
        {
            point = hit.point;

            for (int i = 0; i < spawnAmount; i++) {

                Timer timer = new GameObject("Timer").AddComponent<Timer>();
                timer.RunCountDown(i * 0.1f, GenerateBall);
                

            }
        }


    }

    public void GenerateBall() {

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        float size = Random.Range(minSize, maxSize);
        sphere.transform.localScale = new Vector3(size, size, size);

        float randomX = Random.Range(-spawnRadius / 2, spawnRadius / 2);
        float randomZ = Random.Range(-spawnRadius / 2, spawnRadius / 2);
        Vector3 randomPos = new Vector3(randomX, spawnHeight, randomZ);

        sphere.transform.position = point + randomPos;
        sphere.AddComponent<ProjectileInstance>();
        sphere.GetComponent<Collider>().isTrigger = true;
        
        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.AddForce(Vector3.down * force);

    }
}
