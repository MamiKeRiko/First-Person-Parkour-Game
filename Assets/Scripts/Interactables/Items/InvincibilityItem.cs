using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityItem : ItemBase
{
    public override void OnTriggerWithPlayer(Player player)
    {
        base.OnTriggerWithPlayer(player);
        m_player.SetInvincible(true);
    }

    protected override void ExitAction()
    {
        base.ExitAction();
        m_player.SetInvincible(false);
    }
}
