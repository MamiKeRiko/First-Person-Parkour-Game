using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkItem : ItemBase
{
    public float staminaAmount;

    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        m_player.AddStamina(staminaAmount);
    }
}
