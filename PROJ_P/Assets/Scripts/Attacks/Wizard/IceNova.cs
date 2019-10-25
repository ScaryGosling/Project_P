using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Ice Nova")]
public class IceNova : PlayerAttack
{

    [Header("Ability Specific")]
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private float duration;
    [SerializeField] private Material material;

    public override void RunAttack()
    {
        base.RunAttack();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out hit, 50))
        {

            GenerateIceNova(hit.point);
            
        }
    }


    public void GenerateIceNova(Vector3 position) {

        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.transform.localScale = new Vector3(radius, 0.5f, radius);
        box.GetComponent<Collider>().isTrigger = true;
        box.GetComponent<Renderer>().material = material;
        box.transform.position = position;

        Freeze freeze = box.AddComponent<Freeze>();
        freeze.Timer = duration;
        freeze.StartCoroutine(freeze.FreezeTime());


    }

}
