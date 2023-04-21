using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefillItem : ItemBase
{
    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        WeaponManager.Instance.RefillAllAmmo();
    }
}
