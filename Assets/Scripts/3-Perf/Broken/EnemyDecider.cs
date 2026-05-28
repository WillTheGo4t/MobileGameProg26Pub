using System.Collections.Generic;
using UnityEngine;

public class EnemyDecider : MonoBehaviour
{
    public List<Sprite> Enemies;


    public List<Sprite> GetEnemyList()
    {
        return Enemies;
    }
}
