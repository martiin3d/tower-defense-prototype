using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICannonButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _costText;

    private CannonData.CannonType _cannonType;
    private ICurrencyManager _currencyManager;
    private int _cost;

    /// <summary>
    /// Initializes the cannon button.
    /// Sets up UI texts, icon, and button interactability based on player's coins.
    /// </summary>
    /// <param name="cannonData">Data of the cannon to display.</param>
    /// <param name="callback">Action invoked when the button is clicked, passing the cannon type.</param>
    /// <param name="currencyManager">Reference to the currency manager to track coin changes.</param>
    public void Initialize(CannonData cannonData, Action<CannonData.CannonType> callback, ICurrencyManager currencyManager)
    {
        _currencyManager = currencyManager;
        _cannonType = cannonData.type;
        _cost = cannonData.cost;
        _nameText.SetText(_cannonType.ToString());
        _costText.SetText("{0} coins", _cost);
        _button.image.sprite = cannonData.icon;
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => callback(_cannonType));

        _currencyManager.OnCoinsChanged += UpdateState;
        UpdateState(_currencyManager.GetCoins);
    }

    private void OnDestroy()
    {
        _currencyManager.OnCoinsChanged -= UpdateState;
    }

    /// <summary>
    /// Updates the button's interactable state based on current coins.
    /// Button is enabled only if the player has enough coins to afford the cannon.
    /// </summary>
    /// <param name="currentCoins">Player's current coin amount.</param>
    private void UpdateState(int currentCoins)
    {
        _button.interactable = currentCoins >= _cost;
    }
}