using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExplosiveBase : MonoBehaviour
{
    [Header("Explosion")]
    public GameObject explosionParticles;
    public AudioClip explosionSound;
    public float explosionForce;
    public float explosionRadius;
    public float upwardsModifier;

    public int enemyDamageBase;
    public int friendlyDamageBase;
    public float pushForceBase;
    public float pushForceHorizontalMultiplier;

    public float damageRadius;

    AudioSource source;
    bool hasExploded;

    protected virtual void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void ForceExplode()
    {
        StartCoroutine("Explode");
    }

    protected virtual IEnumerator Explode()
    {
        if (!hasExploded) //avoid duplicate explosions
        {
            hasExploded = true;
            source.PlayOneShot(explosionSound);

            Renderer[] rends = gameObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in rends)
            {
                r.enabled = false;
            }
            gameObject.GetComponent<Collider>().enabled = false;

            if (explosionParticles) Instantiate(explosionParticles, transform.position, Quaternion.identity);
            PhysicsExplode();

            yield return new WaitForSeconds(explosionSound.length);

            Destroy(gameObject);

            yield break;
        }
    }

    protected void PhysicsExplode()
    {
        Collider[] surroundingColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider c in surroundingColliders)
        {
            Rigidbody rb = c.GetComponent<Rigidbody>();
            CharacterBase ch = c.GetComponent<CharacterBase>();

            if (rb)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
            }
            if (ch)
            {
                float dist = Vector3.Distance(transform.position, ch.transform.position);
                if (dist < damageRadius)
                {
                    int dmg;
                    if (ch.characterType == CharacterType.Friendly)
                    {
                        dmg = Mathf.FloorToInt(friendlyDamageBase / dist);
                    }
                    else
                    {
                        dmg = Mathf.FloorToInt(enemyDamageBase / dist);
                    }

                    ch.Hurt(dmg);
                }

                FPSCharacterController fpscc = ch.GetComponent<FPSCharacterController>();

                if (fpscc != null)
                {
                    Vector3 pushVec = (ch.transform.position - transform.position).normalized * pushForceBase / dist;
                    fpscc.Push(new Vector3(pushVec.x * pushForceHorizontalMultiplier, pushVec.y + upwardsModifier, pushVec.z * pushForceHorizontalMultiplier));
                }
            }
        }
    }
}
