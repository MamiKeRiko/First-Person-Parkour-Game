using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitItem : ItemBase
{
    public int healAmount;

    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        player.Heal(healAmount);
    }
    protected override void ApplyEffect()
    {
        base.ApplyEffect();
    }
    protected override void ExitAction()
    {
        base.ExitAction();
    }
}
