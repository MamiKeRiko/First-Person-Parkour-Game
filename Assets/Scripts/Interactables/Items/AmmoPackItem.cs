using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPackItem : ItemBase
{
    public float percent;

    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        WeaponManager.Instance.AddAmmo(percent);
    }
}
