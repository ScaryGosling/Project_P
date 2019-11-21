using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Ice Nova")]
public class IceNova : PlayerAttack
{

    [Header("Ability Specific")]
    [SerializeField] private float radius;
    [SerializeField] private float height;
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
