using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ClassAttackList
{
    public List<ClassAttackSetPair> pairs = new List<ClassAttackSetPair>();

    public AttackSet Get(PlayerClass key) {

        foreach (ClassAttackSetPair pair in pairs) {

            if (pair.key == key) {

                return pair.value;
            }

        }

        throw new System.Exception("Key value pair not found");


    }

}
