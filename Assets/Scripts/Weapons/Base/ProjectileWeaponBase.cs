using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBase : WeaponBase
{
    [Header("Projectile Weapon Specifics")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Renderer loadedProjectileModel;

    protected GameObject m_projectileInstance;
    Renderer[] projectileModelChildren;

    protected override void SetWeaponValues()
    {
        base.SetWeaponValues();
        data.weaponType = WeaponType.Projectile; //always set to projectile to avoid nonsense
    }

    protected override void Start()
    {
        base.Start();
        projectileModelChildren = loadedProjectileModel.GetComponentsInChildren<Renderer>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (m_currentAmmo > 0) SetProjectileVisible(true);
    }

    protected override void Shot()
    {
        base.Shot();

        m_projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        SetProjectileVisible(false);
    }

    protected override IEnumerator Reload()
    {
        StartCoroutine("LoadRocket");
        return base.Reload();
    }

    IEnumerator LoadRocket()
    {
        yield return new WaitForSeconds(data.reloadTime / 2);
        SetProjectileVisible(true);
        yield break;
    }

    protected override IEnumerator Draw()
    {
        if (m_currentAmmo > 0) SetProjectileVisible(true);
        else SetProjectileVisible(false);
        return base.Draw();
    }

    void SetProjectileVisible(bool value)
    {
        if (loadedProjectileModel) loadedProjectileModel.enabled = value;
        if (projectileModelChildren != null)
        {
            foreach (Renderer r in projectileModelChildren)
            {
                r.enabled = value;
            }
        }
    }
}
