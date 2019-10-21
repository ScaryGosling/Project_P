using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventInfo
{
    public string eventDescription;
}

public class DebugEvent : EventInfo
{

}

public class UnitSpawnEvent : EventInfo
{
    public GameObject enemyObject;
    //public int ID;
}

public class UnitDeath : EventInfo
{
    public GameObject enemyObject;
    public float value; 
}

public class GiveResource : EventInfo
{
    public enum Res { Mana, Health, Rage }
    public float fillAmount;
    public Res res;
}

public class ToggleArrowEvent : EventInfo
{
    public bool toggle;
    public GameObject goal;
}

public class MakeDamageEvent  : EventInfo
{
    public GameObject Enemy;
    public float Damage;
}


#region ListenerDraft

//public class WinListener : MonoBehaviour
//{

//    // Start is called before the first frame update
//    void Start()
//    {
//        EventSystem.Current.RegisterListener<WinningEvent>(Winning);
//    }

//    // Update is called once per frame
//    void Winning(WinningEvent winEvent)
//    {
//        Debug.Log("you won! uwu");
//    }
//}
#endregion