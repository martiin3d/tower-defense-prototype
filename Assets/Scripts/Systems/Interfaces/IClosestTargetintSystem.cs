using UnityEngine;

/// <summary>
/// Implements a targeting system that selects the closest enemy to a given position within a specified range.
/// </summary>
public interface IClosestTargetingSystem
{
    /// <summary>
    /// Returns the closest enemy to the specified position within the given range.
    /// </summary>
    /// <param name="cannonPosition">The position to search from.</param>
    /// <param name="range">The maximum distance to search for a enemy.</param>
    /// <returns>The closest valid enemy within range, or null if none are found.</returns>
    Enemy GetTarget(Vector3 cannonPosition, float range);
}