using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSoul : MonoBehaviour, ISoulInterface
{
    //Scriptable Object that has the stats of the warrior inside


    public WarriorSoul()
    {
        LoadStatsIntoPlayer();
    }

    private void Awake()
    {
        GetComponent<Player>().SetSoulInterface(this);
    }

    public void Attack()
    {
        //Warrior Attack
        Debug.Log("WarriorAttack");
    }

    public void LoadStatsIntoPlayer()
    {
        Debug.Log("WarriorStatsLoaded");
    }
}
