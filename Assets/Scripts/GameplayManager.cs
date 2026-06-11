using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }

    public ScriptableCreature SelectedCreature { get; private set; }

    public List<CreatureData> CaughtCreatures = new List<CreatureData>();

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


}
