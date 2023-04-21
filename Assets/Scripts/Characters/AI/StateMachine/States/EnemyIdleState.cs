using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    public override void EnterState()
    {
        weaponAnimator.SetTrigger("Idle");
    }

    public override void ExitState()
    {
        weaponAnimator.ResetTrigger("Idle");
    }
}
