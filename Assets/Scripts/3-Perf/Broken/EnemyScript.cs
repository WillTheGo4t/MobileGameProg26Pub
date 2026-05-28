using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject _particles;
    [SerializeField] GameObject _enemyDecider;
    GameObject _scoreObject;


    [SerializeField] float _speed = 1.0f;
    [SerializeField] float _changeInterval = 0.5f;
    float _timer =0.5f;

    private Vector3 _direction;

    bool _hit = false;

    void Start()
    {
        //Get Enemy Decider
        GameObject enemyDecider = Instantiate(_enemyDecider, this.transform.position, this.transform.rotation, this.transform);
        EnemyDecider enemyDeciderScript = enemyDecider.GetComponent<EnemyDecider>();

        //Get List of enemy Types
        List<Sprite> enemyTypes = new List<Sprite>();
        enemyTypes = enemyDeciderScript.GetEnemyList();

        //Decide on Enemy and apply
        int random = Random.Range(0, enemyTypes.Count);
        GetComponentInChildren<SpriteRenderer>().sprite = enemyTypes[random];

        //Make sure we have white set as Color
         GetComponentInChildren<SpriteRenderer>().material.color = Color.white;
    }

    void Update()
    {
        if (_hit == true)
            DestroyEnemy();
    }


    void FixedUpdate()
    {
        MoveRandom();
    }

    void MoveRandom()
    {
        _timer += Time.deltaTime;

        //change direction
        if (_timer >= _changeInterval)
        {
            _timer -= _changeInterval;
            _direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }
        //move in a random direction 
        transform.position += _direction * _speed * Time.deltaTime;
    }
    void DestroyEnemy()
    {
        _hit = false;
        //Play particles
        Instantiate(_particles, this.transform.position, this.transform.rotation, this.transform);
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        particleSystem.Play();

        //Destroy self delayed, so particles can play
        Destroy(this.gameObject, 0.5f);
    }

    public void PointerDown(BaseEventData eventData)
    {
        _hit = true;
        //add score
        _scoreObject = GameObject.Find("Score");
        _scoreObject.GetComponent<Score>().AddScore();

    }
}
