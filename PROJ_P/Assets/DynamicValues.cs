using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DynamicValues : MonoBehaviour
{
    [SerializeField] private AttackSet[] attackSets;

    private string mysticPath, fighterPath;

    public void Awake()
    {
        //Mystic
        if(File.Exists(Application.persistentDataPath + "/Mystic.json"))
        {
            mysticPath = File.ReadAllText(Application.persistentDataPath + "/Mystic.json");
            JsonData mystic = JsonUtility.FromJson<JsonData>(mysticPath);
            attackSets[0].originalStats.movementSpeed = mystic.movementSpeed;
            attackSets[0].originalStats.resistanceMultiplier = mystic.resistanceMultiplier;
            attackSets[0].originalStats.attackSpeed = mystic.attackSpeed;
            attackSets[0].originalStats.attackDamage = mystic.attackDamage;
        }
        else
        {
            string json = JsonUtility.ToJson(attackSets[0].originalStats);
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Mystic.json", true);
            json = json.Replace(",", ",\n");
            writer.WriteLine(json);
            writer.Close();

            writer = new StreamWriter(Application.persistentDataPath + "/Mystic-bu.json", true);
            json = json.Replace(",", ",\n");
            writer.WriteLine(json);

            writer.Close();
        }



        //Fighter
        if (File.Exists(Application.persistentDataPath + "/Fighter.json"))
        {
            fighterPath = File.ReadAllText(Application.persistentDataPath + "/Fighter.json");
            JsonData fighter = JsonUtility.FromJson<JsonData>(fighterPath);
            attackSets[1].originalStats.movementSpeed = fighter.movementSpeed;
            attackSets[1].originalStats.resistanceMultiplier = fighter.resistanceMultiplier;
            attackSets[1].originalStats.attackSpeed = fighter.attackSpeed;
            attackSets[1].originalStats.attackDamage = fighter.attackDamage;
        }
        else
        {
            string json = JsonUtility.ToJson(attackSets[0].originalStats);
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Fighter.json", true);
            json = json.Replace(",", ",\n");
            writer.WriteLine(json);
            writer.Close();

            writer = new StreamWriter(Application.persistentDataPath + "/Fighter-bu.json", true);
            json = json.Replace(",", ",\n");
            writer.WriteLine(json);

            writer.Close();
        }





    }

    private class JsonData
    {
        public float movementSpeed;
        public float resistanceMultiplier;
        public float attackSpeed;
        public float attackDamage;

    }
}
