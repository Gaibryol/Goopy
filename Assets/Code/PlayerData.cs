using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
[System.Serializable]
public class PlayerData : ScriptableObject
{
    public List<SavedGoopy> SavedGoopies;
    public List<SavedUpgrade> SavedUpgrades;

    public PlayerData()
    {
        SavedGoopies = new List<SavedGoopy>();
        SavedUpgrades = new List<SavedUpgrade>();
    }

    public void Save(List<SavedGoopy> savedGoopies, List<SavedUpgrade> savedUpgrades)
    {
        SavedGoopies = savedGoopies;
        SavedUpgrades = savedUpgrades;
    }
}

[System.Serializable]
public class SavedUpgrade
{
    public Upgrade Upgrade;
    public string LastApplied;

    public SavedUpgrade(Upgrade upgrade)
    {
        Upgrade = upgrade;
        //LastApplied = timeString;
    }
}

[System.Serializable]
public class SavedGoopy
{
    public int Age;
    public string Type;

    public SavedGoopy(int age, string type)
    {
        Age = age;
        Type = type;
    }
}
