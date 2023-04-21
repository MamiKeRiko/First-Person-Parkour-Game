using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEGrenadeBehavior : ProjectileWeaponBase
{
    [Header("Grenade")]
    public float throwForce;

    protected override void Shot()
    {
        base.Shot();

        Vector3 throwVector = transform.forward * throwForce + Vector3.up * throwForce / 8;
        m_projectileInstance.GetComponent<GrenadeBehavior>().AddThrowForce(throwVector);

        FinishShot();
    }
}
