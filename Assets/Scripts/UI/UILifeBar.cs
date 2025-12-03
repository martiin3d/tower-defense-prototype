using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILifeBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private TextMeshProUGUI _text;

    /// <summary>
    /// Updates the life bar UI elements based on current and maximum life values.
    /// </summary>
    /// <param name="life">Current life value.</param>
    /// <param name="maxLife">Maximum life value.</param>
    public void UpdateLife(float life, float maxLife)
    {
        if (_bar == null)
        {
            Debug.LogWarning("[UILifeBar] bar is not assigned.");
            return;
        }

        if (maxLife <= 0)
        {
            return;
        }

        float normalizedLife = Mathf.Clamp01(life / maxLife);
        _bar.fillAmount = normalizedLife;
        _text?.SetText("{0}/100", normalizedLife * 100);
    }
}
