using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _spawnTime = 1f;

    [SerializeField] int _maximumEnemies = 10;

    public GameObject Enemy;

    float _timer;

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _spawnTime)
        {
            _timer -= _spawnTime;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < 10)
                Instantiate(Enemy);
        }
    }
}
