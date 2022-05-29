using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeEntry
{
    [SerializeField] int cost;
    [SerializeField] float newValue;

    public int Cost
    {
        get
        {
            return cost;
        }
    }

    public float NewValue
    {
        get
        {
            return newValue;
        }
    }
}

public class Upgrades : MonoBehaviour
{
    [SerializeField] UpgradeEntry[] upgradeList;


    public UpgradeEntry[] UpgradeList
    {
        get
        {
            return upgradeList;
        }
    }
}
