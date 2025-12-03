
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIReadySetGo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _secondsBetweenText;

    public Action OnAnimationFinish;

    public void StartAnimation(int wave)
    {
        StartCoroutine(PlayReadyAnim(wave));
    }

    private IEnumerator PlayReadyAnim(int wave)
    {
        _text.SetText("WAVE {0}", wave + 1);
        yield return new WaitForSeconds(_secondsBetweenText);

        _text.SetText("READY");
        yield return new WaitForSeconds(_secondsBetweenText);

        _text.SetText("SET");
        yield return new WaitForSeconds(_secondsBetweenText);

        _text.SetText("GO");
        yield return new WaitForSeconds(_secondsBetweenText);

        OnAnimationFinish?.Invoke();

        _text.SetText(string.Empty);
        gameObject.SetActive(false);
    }
}
