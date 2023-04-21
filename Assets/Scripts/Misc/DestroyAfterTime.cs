using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifeTime = 5;

    void Start()
    {
        StartCoroutine("WaitAndDestroy");
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
