using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")] //right click in our hierarchy / project plane to create this unit

public class ScriptableUnit : ScriptableObject        //not the actual unit, just an encasing
{
    public Faction Faction;
    public BaseUnit UnitPrefab;
    
}


public enum Faction
{
    Hero = 0,
    Enemy = 1
}