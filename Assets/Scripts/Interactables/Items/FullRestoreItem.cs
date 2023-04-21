using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullRestoreItem : ItemBase
{
    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        m_player.Restore();
        WeaponManager.Instance.RefillAllAmmo();
    }
}
