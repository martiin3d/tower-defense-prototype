using System;
using TMPro;
using UnityEngine;

public class UICannonSelector : MonoBehaviour
{
    [SerializeField] private UICannonButton _buttonPrefab;
    [SerializeField] private Transform _content;
    [SerializeField] private TextMeshProUGUI _placingText;

    public Action<CannonData.CannonType> OnCannonSelected;

    /// <summary>
    /// Initializes the selector by creating a button for each cannon in the database.
    /// Sets up each button with the currency manager to manage interactability.
    /// </summary>
    /// <param name="database">Cannon database with cannon configurations.</param>
    /// <param name="currencyManager">Currency manager to track player's coins for button states.</param>
    public void Initialize(CannonDatabase database, ICurrencyManager currencyManager)
    {
        foreach (var cannon in database.CannonDataList)
        {
            UICannonButton button = Instantiate(_buttonPrefab, _content);
            button.Initialize(cannon, OnCannonButtonClick, currencyManager);
        }
    }

    /// <summary>
    /// Shows or hides the placing text UI element.
    /// </summary>
    /// <param name="show">A value indicating whether the placing text should be visible. Set to <see langword="true"/> to show the text;
    /// set to <see langword="false"/> to hide it.</param>
    public void ShowPlacingText(bool show)
    {
        _placingText.gameObject.SetActive(show);
    }

    /// <summary>
    /// Sets the text to be displayed for the current placing operation.
    /// </summary>
    /// <param name="text">The text to display during the placing operation. Can be null or empty to clear the current text.</param>
    public void SetPlacingText(CannonData.CannonType cannonType)
    {
        _placingText.SetText($"Placing {cannonType.ToString()} Cannon");
    }

    private void OnCannonButtonClick(CannonData.CannonType type)
    {
        OnCannonSelected?.Invoke(type);
    }
}
