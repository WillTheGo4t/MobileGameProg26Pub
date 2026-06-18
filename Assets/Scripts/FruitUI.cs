using UnityEngine;
using TMPro;

public class FruitUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _countText;

    void Start()
    {
        UpdateUI(GameplayManager.Instance.FruitCount);

        GameplayManager.Instance.OnFruitCountChanged += UpdateUI;
    }

    void OnDestroy()
    {
        if (GameplayManager.Instance != null)
        {
            GameplayManager.Instance.OnFruitCountChanged -= UpdateUI;
        }
    }

    void UpdateUI(int newCount)
    {
        _countText.text = newCount.ToString();
    }
}