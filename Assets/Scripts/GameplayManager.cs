using System;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public ScriptableCreature SelectedCreature { get; private set; }
    public List<CreatureData> CaughtCreatures = new List<CreatureData>();
    public int FruitCount { get; private set; }

    public event Action<int> OnFruitCountChanged;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void AddCaughtCreature(CreatureData caughtCreature)
    {
        CaughtCreatures.Add(caughtCreature);
    }

    public void SetSelectedCreature(ScriptableCreature scriptableCreature)
    {
        SelectedCreature = scriptableCreature;
    }

    public void UnselectCreature()
    {
        SelectedCreature = null;
    }

    public void AddFruit(int amount)
    {
        FruitCount += amount;
        OnFruitCountChanged?.Invoke(FruitCount);
    }

    public bool TryUseFruit()
    {
        if (FruitCount > 0)
        {
            FruitCount--;
            OnFruitCountChanged?.Invoke(FruitCount);
            return true;
        }
        return false;
    }
}