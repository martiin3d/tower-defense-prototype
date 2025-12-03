using System;

/// <summary>
/// Manages the player's in-game currency (coins), including adding, spending, and resetting coin values.
/// Notifies listeners when the coin amount changes.
/// </summary>
public interface ICurrencyManager
{
    /// <summary>
    /// Gets the current number of coins.
    /// </summary>
    int GetCoins { get; }

    /// <summary>
    /// Event invoked whenever the coin amount changes.
    /// The new coin amount is passed as a parameter.
    /// </summary>
    Action<int> OnCoinsChanged { get; set; }

    /// <summary>
    /// Checks if the player has enough coins to spend a specified amount.
    /// </summary>
    /// <param name="amount">The number of coins to check against.</param>
    /// <returns>True if there are enough coins; otherwise, false.</returns>
    bool HasEnough(int amount);

    /// <summary>
    /// Attempts to spend a specified amount of coins. 
    /// Returns false if there aren't enough coins.
    /// </summary>
    /// <param name="amount">The number of coins to spend.</param>
    /// <returns>True if the coins were successfully spent; otherwise, false.</returns>
    bool Spend(int amount);

    /// <summary>
    /// Adds a specified amount of coins.
    /// </summary>
    /// <param name="amount">The number of coins to add.</param>
    void Add(int amount);

    /// <summary>
    /// Resets the coin amount to the starting value.
    /// </summary>
    void Reset();
}