using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{   
    int _score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        GetComponentInChildren<TextMeshProUGUI>().text = "Score "  + _score.ToString();
    }

    public void AddScore()
    {
        _score++;
    }
}
