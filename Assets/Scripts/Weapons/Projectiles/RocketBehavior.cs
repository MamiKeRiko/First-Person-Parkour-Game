using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : ExplosiveBase
{
    [Header("Rocket")]
    public float speed;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine("Explode");
    }
       
    public void SetTarget(Vector3 newTarget)
    {
        Vector3 newForward = (newTarget - transform.position).normalized;
        transform.forward = newForward;
    }
}
