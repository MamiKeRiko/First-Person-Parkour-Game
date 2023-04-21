using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponInput : MonoBehaviour
{
    WeaponManager weaponManager;
    WeaponBase[] weapons;

    bool init = false;

    private void OnEnable()
    {
        weaponManager = GetComponent<WeaponManager>();
        StartCoroutine("GetWeapons");
    }

    IEnumerator GetWeapons()
    {
        while (weapons == null)
        {
            weapons = weaponManager.GetWeapons();
            yield return new WaitForEndOfFrame();
        }

        init = true;
        yield break;
    }

    private void Update()
    {
        if (init)
        {
            WeaponBase weapon = weapons[weaponManager.currentWeapon];

            if (Input.GetButton("Fire1"))
            {
                weapon.RequireShot = true;
            }
            if (Input.GetButtonUp("Fire1"))
            {
                weapon.RequireShot = false;
                weapon.StopHolding();
            }
            if (Input.GetButtonDown("Reload"))
            {
                weapon.RequireReload = true;
            }
        }
    }
}
