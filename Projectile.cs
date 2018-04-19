using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum proType
{
    rock, arrow, fireball
};

public class Projectile : MonoBehaviour {


    //Serialized Fields
    [SerializeField]
    private int attackStrength;
    [SerializeField]
    private proType projectileType;


    //Public Variables
    public int AttackStrength
    {
        get
        {
            return attackStrength;
        }
        set
        {
            attackStrength = value;
        }
    }

    public proType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }
}
