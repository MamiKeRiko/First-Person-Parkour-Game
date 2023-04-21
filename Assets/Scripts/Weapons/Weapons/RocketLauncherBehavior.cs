using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherBehavior : ProjectileWeaponBase
{
    protected override void Shot()
    {
        base.Shot();

        Ray ray = new Ray(m_raycastSpot.position, m_raycastSpot.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
            m_projectileInstance.GetComponent<RocketBehavior>().SetTarget(hitInfo.point);

        FinishShot();
    }
}
