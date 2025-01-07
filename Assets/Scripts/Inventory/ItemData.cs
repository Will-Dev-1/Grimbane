using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[CreateAssetMenu]

public class ItemData : ScriptableObject
{
    public string displayName;
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;
    public bool collected = false;

    //Save System
    public string id;

    //Phantom Dash
    public float dashDelayModifier;
    public float dashLengthModifier;

    //Ranged Attack
    public bool isRangedAttack;
    public float projectileDamage;
    public float healthCostPerShot;
    public GameObject projectilePrefab;

    //Melee Weapons
    public bool isWeapon;
    public float wepDmgMod;
    public float wepSpdMod;

    //Berserker Ring
    public float damageMultiplier;
    public bool disableLifesteal;

    //Secret Room Finder
    public bool isSecretRoomFinder;
}
