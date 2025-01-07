using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastWep : WeaponBase
{
    float wepDmgMod = -2.0f;

    float wepSpdMod = .4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override float getWepDmg()
    {
        return wepDmgMod;
    }

    public override float getWepSpd()
    {
        return wepSpdMod;
    }
}
