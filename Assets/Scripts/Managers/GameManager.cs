using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public bool debugMode;

    WeaponManager weaponManager;
    UIManager uiManager;
    Player player;

    bool weaponManagerInit = false;
    bool uiManagerInit = false;

    // PLAYER //

    public void SetPlayer(Player instance)
    {
        player = instance;
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void PlayerDied()
    {
        Debug.Log("You died!");
    }
}
