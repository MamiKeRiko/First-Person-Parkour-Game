using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : StateBase
{
    protected Animator weaponAnimator;

    public override void InitState(CharacterBase character, StateMachine stateMachine)
    {
        base.InitState(character, stateMachine);

        try
        {
            Enemy e = (Enemy)m_character;
            weaponAnimator = e.equippedWeapon.GetComponent<Animator>();
        }
        catch
        {
            Debug.LogError("Enemy State attached to non-enemy entity");
        }
    }
}
