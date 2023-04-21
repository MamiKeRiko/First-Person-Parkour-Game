using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase : MonoBehaviour
{
    protected CharacterBase m_character;
    protected StateMachine m_stateMachine;

    public virtual void InitState(CharacterBase character, StateMachine stateMachine)
    {
        m_character = character;
        m_stateMachine = stateMachine;

        enabled = false;
    }

    public virtual void EnterState(){}
    public virtual void ExitState(){}
}
