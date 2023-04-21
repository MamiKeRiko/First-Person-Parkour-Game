using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObjectBase : MonoBehaviour
{
    public abstract void OnTriggerWithPlayer(Player player);
}
