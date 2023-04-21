using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected StateBase m_currentState;

    public StateBase GetCurrentState() { return m_currentState; }

    public void InitStateMachine(StateBase initialState)
    {
        m_currentState = initialState;
        m_currentState.enabled = true;
        m_currentState.EnterState();
    }

    public void EnterState(StateBase newState)
    {
        m_currentState.ExitState();
        m_currentState.enabled = false;

        m_currentState = newState;
        m_currentState.enabled = true;
        m_currentState.EnterState();
    }
}
