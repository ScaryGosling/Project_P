using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackBehaviour : MonoBehaviour
{

    private List<PlayerAttack> playerAttacks = new List<PlayerAttack>();
    [SerializeField] private PlayerAttack[] activeAttacks = new PlayerAttack[3];
    private PlayerAttack activeAttack;
    [SerializeField] private Transform spawnPoint;

    public void Start()
    {
        CacheComponents();
    }

    public void CacheComponents() {
        activeAttack = activeAttacks[0];
    }

    public void Update() {

        if (Input.GetMouseButtonDown(0)) {

            ExecuteAttack();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectAttack(2);
        }


    }

    public void ExecuteAttack() {
        activeAttack.Execute(spawnPoint);
    }

    public void SelectAttack(int selectedAttack) {

        if(activeAttacks[selectedAttack])
            activeAttack = activeAttacks[selectedAttack];

    }



}
