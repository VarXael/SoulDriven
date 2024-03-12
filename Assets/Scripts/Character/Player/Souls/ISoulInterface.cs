using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoulInterface
{
    //Attack function for each soul class
    public void Attack();
    //This function serves to load the stats of the current soul into the player stats
    //Example: if i switch to a warrior, i want the stats of the player to become equal to: BasePlayerStats + WarriorStats
    public void LoadStatsIntoPlayer();
}
