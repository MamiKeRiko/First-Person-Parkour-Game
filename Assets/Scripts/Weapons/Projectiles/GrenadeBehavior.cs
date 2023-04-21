using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBehavior : ExplosiveBase
{
    [Header("Grenade")]
    public int lifetime;

    public Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        //rb = GetComponent<Rigidbody>();
        Invoke("ExplodeGrenade", lifetime);
    }

    void ExplodeGrenade()
    {
        StartCoroutine("Explode");
    }

    public void AddThrowForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}
