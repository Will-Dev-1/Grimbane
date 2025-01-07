using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class enemyBase : MonoBehaviour, IDamageable
{
    //Inherited Methods
    public abstract void takeDmg(float dmg);

    public abstract float getDmg();

    public abstract void playerSight(bool sight);

    public abstract void setPlayerObj(GameObject Obj);

    public abstract void setWaypoints(Transform wp1, Transform wp2);
}
