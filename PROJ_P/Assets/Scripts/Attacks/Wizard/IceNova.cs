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
    private Collider[] hitColliders;

    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        duration = durationPerLevel[CurrentLevel];
    }
    public override void RunAttack()
    {
        base.RunAttack();
        hitColliders = null;
        player.AnimationTrigger("IceField");
        hitColliders = Physics.OverlapSphere(player.transform.position, radius);
        //GenerateIceNova(player.transform.position);

        foreach (Collider collider in hitColliders) {

            if (collider.CompareTag("Enemy"))
            {
                Unit unit = collider.GetComponent<Unit>();
                unit.agent.enabled = false;
                Instantiate(icePrefab, unit.transform.position - Vector3.up * unit.transform.localScale.y / 2, unit.transform.rotation, unit.transform);
            }
        }

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(duration, Reset, Timer.TimerType.DELAY);

    }

    public void Reset()
    {
        foreach (Collider collider in hitColliders)
        {


            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Unit>().agent.enabled = true;
                Destroy(collider.transform.GetChild(collider.transform.childCount-1).gameObject);
            }

        }
    }


}
