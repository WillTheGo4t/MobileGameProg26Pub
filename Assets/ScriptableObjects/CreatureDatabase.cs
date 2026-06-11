using System.Collections.Generic;
using UnityEngine;



public class CreatureDatabase : MonoBehaviour
{
    public static CreatureDatabase Instance { get; private set; }

    [SerializeField] List<ScriptableCreature> _allCreatures;

    private Dictionary<int, ScriptableCreature> _creatureDictionary;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this.gameObject);


        _creatureDictionary = new Dictionary<int, ScriptableCreature>();
        foreach (var creature in _allCreatures)
        {
            if (!_creatureDictionary.ContainsKey(creature.ID))
            {
                _creatureDictionary.Add(creature.ID, creature);
            }
        }
    }

    public ScriptableCreature GetScriptableCreatureSOByID(int id)
    {
        if (_creatureDictionary.TryGetValue(id, out ScriptableCreature data))
        {
            return data;
        }

        Debug.LogError($"Creature mit ID {id} wurde nicht in der Datenbank gefunden");
        return null;
    }

}
