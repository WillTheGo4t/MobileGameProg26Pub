using System.Linq;
using TMPro;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{


    [System.Serializable]
    public class NamedColor
    {
        public Color color;
        public string name;
    }

    [SerializeField] NamedColor[] _namedColors;


    Color[] _colors = { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow };
    string[] _colorNames = { "Red", "Green", "Blue", "Cyan", "Magenta", "Yellow" };

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] TextMeshPro _oldColorText;

    string _oldColor = "white";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("UpdateColor", 0f, 1f);

    }

    void UpdateColor()
    {

        // Random Nummer generieren
        int randomNumber = Random.Range(0, _namedColors.Length);

        //Sprite Farbe setzen
        _spriteRenderer.color =  _namedColors[randomNumber].color;
        //Text mit alter Farbe setzen
        _oldColorText.text = _oldColor;

        // Color Name setzen
        string newColorName =_namedColors[randomNumber].name;
        // Variable updaten, für nächstes mal
        _oldColor = newColorName;

        Debug.Log("Time: " + Time.realtimeSinceStartup + " old Color:  " + _oldColor + " new Color: " + newColorName);

    }

}
