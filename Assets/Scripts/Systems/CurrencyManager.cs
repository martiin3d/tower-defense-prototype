using System;

/// <summary>
/// Manages the player's in-game currency (coins), including adding, spending, and resetting coin values.
/// Notifies listeners when the coin amount changes.
/// </summary>
public class CurrencyManager : ICurrencyManager
{
    /// <summary>
    /// Gets the current number of coins.
    /// </summary>
    public int GetCoins => _coins;

    /// <summary>
    /// Event invoked whenever the coin amount changes.
    /// The new coin amount is passed as a parameter.
    /// </summary>
    public Action<int> OnCoinsChanged { get; set; }

    private int _coins;
    private int _startingCoins;

    /// <summary>
    /// Creates a new instance of the currency manager with a specified starting coin amount.
    /// </summary>
    /// <param name="startingCoins">The initial number of coins.</param>
    public CurrencyManager(int startingCoins)
    {
        _startingCoins = startingCoins;
        _coins = _startingCoins;
    }

    /// <summary>
    /// Checks if the player has enough coins to spend a specified amount.
    /// </summary>
    /// <param name="amount">The number of coins to check against.</param>
    /// <returns>True if there are enough coins; otherwise, false.</returns>
    public bool HasEnough(int amount)
    {
        return _coins >= amount;
    }

    /// <summary>
    /// Attempts to spend a specified amount of coins. 
    /// Returns false if there aren't enough coins.
    /// </summary>
    /// <param name="amount">The number of coins to spend.</param>
    /// <returns>True if the coins were successfully spent; otherwise, false.</returns>
    public bool Spend(int amount)
    {
        if (!HasEnough(amount))
        {
            return false;
        }

        _coins -= amount;
        OnCoinsChanged?.Invoke(_coins);
        return true;
    }

    /// <summary>
    /// Adds a specified amount of coins.
    /// </summary>
    /// <param name="amount">The number of coins to add.</param>
    public void Add(int amount)
    {
        _coins += amount;
        OnCoinsChanged?.Invoke(_coins);
    }

    /// <summary>
    /// Resets the coin amount to the starting value.
    /// </summary>
    public void Reset()
    {
        _coins = _startingCoins;
        OnCoinsChanged?.Invoke(_coins);
    }
}