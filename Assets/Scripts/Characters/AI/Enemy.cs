using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterBase 
{
    [Header("Enemy")]
    public StateBase initialState;
    public GameObject equippedWeapon;
    //public GameObject dropWeaponPrefab;
    public Transform shootSpot;

    WeaponBase weapon;
    NavMeshAgent agent;

    StateMachine sm;
    StateBase[] states;
    
    SenseSight sight;
    bool canSeePlayer;

    private void Start()
    {
        sm = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        weapon = equippedWeapon.GetComponent<WeaponBase>();
        
        states = GetComponents<StateBase>();
        foreach (StateBase state in states)
        {
            state.InitState(this, sm);
        }
        sm.InitStateMachine(initialState);

        sight = GetComponent<SenseSight>();
        sight.SightTriggered += SeePlayer;
        sight.SightLost += StopSeeingPlayer;

        characterType = CharacterType.Enemy;

        InitWeapon();
    }

    void InitWeapon()
    {
        weapon.m_raycastSpot = shootSpot;
        weapon.audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        characterType = CharacterType.Enemy;
    }

    void SeePlayer()
    {
        Debug.Log("Player detected");
        canSeePlayer = true;
    }

    void StopSeeingPlayer()
    {
        Debug.Log("Player lost");
        canSeePlayer = false;
    }

    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }
}
