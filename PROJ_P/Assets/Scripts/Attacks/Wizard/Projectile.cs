using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Projectile")]
public class Projectile : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float force;

    public override void Execute()
    {
        base.Execute();
    }
    

    public override void RunAttack()
    {
        base.RunAttack();
        Transform spawnPoint = Player.instance.GetSpawnPoint();

        GameObject ball = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        AimAssist();
        ball.GetComponent<Rigidbody>().AddForce(AimAssist() * force + Player.instance.GetComponent<Rigidbody>().velocity);
        ball.GetComponent<ProjectileInstance>().SetPower(damage, magnitude);
    }


    public Vector3 AimAssist()
    {
        Vector3 direction = Player.instance.GetSpawnPoint().TransformDirection(Vector3.forward);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.point + hit.collider.name);

            if(Player.instance.GetSettings().UseAimAssist)
                direction = GetDirection(Physics.OverlapSphere(hit.point, Player.instance.GetSettings().GetAimAssistRange()));

        }

        return direction.normalized;
    }

    public Vector3 GetDirection(Collider[] inRange)
    {
        Vector3 playerPos = Player.instance.transform.position;
        Collider closestEnemy = inRange[0];

        for(int i = 1; i < inRange.Length; i++)
        {
            float myDistance = Vector3.Distance(playerPos, inRange[i].transform.position);
            if (myDistance < Vector3.Distance(closestEnemy.transform.position, playerPos) && inRange[i].CompareTag("Enemy"))
            {
                closestEnemy = inRange[i].transform.root.GetComponent<Collider>();
            }
        }

        if (closestEnemy.CompareTag("Enemy"))
            return closestEnemy.transform.position - playerPos;
        else
            return Player.instance.GetSpawnPoint().TransformDirection(Vector3.forward);

    }

}
