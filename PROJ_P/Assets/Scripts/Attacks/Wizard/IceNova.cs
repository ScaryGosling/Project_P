using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Ice Nova")]
public class IceNova : PlayerAttack
{

    [Header("Ability Specific")]
    [SerializeField] private float radius;
    [SerializeField] private float height;
    [SerializeField] private GameObject icePrefab;
    private float duration;
    [SerializeField] private Material material;
    [SerializeField] private List<float> durationPerLevel = new List<float>();


    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        duration = durationPerLevel[CurrentLevel];
    }
    public override void RunAttack()
    {
        base.RunAttack();
        GenerateIceNova(Player.instance.transform.position);
    }


    public void GenerateIceNova(Vector3 position) {

        GameObject box = BowoniaPool.instance.GetFromPool(PoolObject.ICE_NOVA);
        box.transform.localScale = new Vector3(radius, 0.5f, radius);
        box.GetComponent<Collider>().isTrigger = true;
        box.GetComponent<Renderer>().material = material;
        box.transform.position = position;

        Freeze freeze = box.GetComponent<Freeze>();
        freeze.Timer = duration;
        freeze.Damage = damage;
        freeze.Magnitude = magnitude;
        freeze.StartCoroutine(freeze.FreezeTime());


    }

}
