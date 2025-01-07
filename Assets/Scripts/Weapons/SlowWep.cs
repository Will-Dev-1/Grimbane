using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWep : WeaponBase
{
    float wepDmgMod = 2.0f;

    float wepSpdMod = 1.5f;
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
