using TMPro;
using UnityEngine;

public class UIHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    private ICurrencyManager _currencyManager;

    public void Initialize(ICurrencyManager currencyManager)
    {
        _currencyManager = currencyManager;
        _currencyManager.OnCoinsChanged += OnCoinsChangedEvent;
    }

    private void OnDestroy()
    {
        _currencyManager.OnCoinsChanged -= OnCoinsChangedEvent;
    }

    private void OnCoinsChangedEvent(int coins)
    {
        _coinsText.SetText(coins.ToString());
    }
}
