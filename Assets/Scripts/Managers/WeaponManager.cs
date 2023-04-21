using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : TemporalSingleton<WeaponManager>
{
    public GameObject[] weapons;
    public int currentWeapon { get; private set; }
    public bool AreWeaponsInit { get; private set; }

    private WeaponBase[] weaponCC;
    private int[,] weaponValues;
    private int[,] defaultWeaponValues;

    private int numberOfWeapons;
    private int activeWeapons;

    public WeaponBase[] GetWeapons() { return weaponCC; }

    private WeaponInfo weaponInfo;

    public override void Awake()
    {
        base.Awake();
        numberOfWeapons = weapons.Length;
    }

    private void Start()
    {
        weaponInfo = Resources.Load<WeaponInfo>("WeaponList");
        
        for(int i = 0; i < weaponInfo.list.Count; i++)
        {
            Debug.Log(weaponInfo.list[i].name);
        }
    }

    void Init()
    {
        weaponCC = new WeaponBase[weapons.Length];
        weaponValues = new int[weapons.Length, 2];
        defaultWeaponValues = new int[weapons.Length, 2];
        currentWeapon = 0;

        int i = 0;
        foreach (GameObject go in weapons)
        {
            weaponCC[i] = go.GetComponent<WeaponBase>();

            weaponCC[i].enabled = false;
            weaponCC[i].SetWeaponData(weaponInfo.list[i]);
            weaponCC[i].weaponOwner = GameManager.Instance.GetPlayer();
            SaveWeaponAmmo(i);
            SaveWeaponDefaults(i);
            i++;
        }

        Invoke("SetWeapon", 0.1f);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0.05)
        {
            SwitchWeapon(1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < -0.05)
        {
            SwitchWeapon(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetWeapon(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetWeapon(4);
        }
    }

    private bool CheckWeaponsInit()
    {
        if (activeWeapons >= numberOfWeapons)
        {
            Debug.Log("All weapons fully loaded");
            AreWeaponsInit = true;
            return true;
        }
        else return false;
    }

    public void WeaponNotifyInit(GameObject notifier)
    {
        activeWeapons++;
        notifier.SetActive(false);

        if (CheckWeaponsInit())
        {
            Init();
        }
    }


    void SetWeapon()
    {
        if (!weapons[currentWeapon].activeInHierarchy)
        {
            SaveWeaponAmmo(currentWeapon);

            foreach (GameObject obj in weapons)
            {
                obj.SetActive(false);
            }
            weapons[currentWeapon].SetActive(true);
            weaponCC[currentWeapon].enabled = true;

            SetWeaponAmmo();
        }
    }

    public void SetWeapon(int index)
    {
        currentWeapon = index;
        SetWeapon();
    }

    void SetWeaponAmmo()
    {
        int[] values = new int[2];
        values[0] = weaponValues[currentWeapon, 0];
        values[1] = weaponValues[currentWeapon, 1];

        weaponCC[currentWeapon].SetAmmoValues(values);
    }

    public void AddAmmo(float percent)
    {
        int amount = Mathf.RoundToInt(defaultWeaponValues[currentWeapon, 1] * percent / 100);
        Debug.Log(amount);
        weaponValues[currentWeapon, 1] += amount;
        if (weaponValues[currentWeapon, 1] > defaultWeaponValues[currentWeapon, 1]) weaponValues[currentWeapon, 1] = defaultWeaponValues[currentWeapon, 1];
        SetWeaponAmmo();
    }

    public void RefillAllAmmo()
    {
        for(int i = 0; i < weaponCC.Length; i++)
        {
            int[] values = new int[2];
            values[0] = weaponValues[i, 0] = defaultWeaponValues[i, 0];
            values[1] = weaponValues[i, 1] = defaultWeaponValues[i, 1];

            weaponCC[i].SetAmmoValues(values);
        }

        UIManager.Instance.ChangeAmmoText(weaponValues[currentWeapon, 0], weaponValues[currentWeapon, 1]);
    }

    public void SwitchWeapon(int amount)
    {
        currentWeapon += amount;
        currentWeapon = Mathf.Clamp(currentWeapon, 0, weapons.Length - 1);
        SetWeapon();
    }

    void SaveWeaponAmmo(int index)
    {
        int[] values = weaponCC[index].GetAmmoValues();
        weaponValues[index, 0] = values[0];
        weaponValues[index, 1] = values[1];
    }

    void SaveWeaponDefaults(int index)
    {
        int[] values = weaponCC[index].GetAmmoValues();
        defaultWeaponValues[index, 0] = values[0];
        defaultWeaponValues[index, 1] = values[1];
    }
}
