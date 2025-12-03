using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIHud _hud;
    [SerializeField] private UIReadySetGo _readySetGoPanel;
    [SerializeField] private Button _retryButton;
    [SerializeField] private GameObject _losePopup;
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private GameObject _cannonPlacer;

    public Action OnRetryButton;
    public Action OnReadySetGoFinish
    {
        get
        {
            return _readySetGoPanel.OnAnimationFinish;
        }
        set
        {
            _readySetGoPanel.OnAnimationFinish = value;
        }
    }

    public void Initialize(ICurrencyManager currencyManager)
    {
        _hud.Initialize(currencyManager);
    }

    private void OnEnable()
    {
        _retryButton.onClick.AddListener(OnRetryButtonClick);
        _readySetGoPanel.OnAnimationFinish += OnReadySetGoFinishEvent;
    }

    private void OnDisable()
    {
        _retryButton.onClick.RemoveListener(OnRetryButtonClick);
        _readySetGoPanel.OnAnimationFinish -= OnReadySetGoFinishEvent;
    }

    public void StartReadySetGo(int wave)
    {
        _cannonPlacer.gameObject.SetActive(false);
        _readySetGoPanel.gameObject.SetActive(true);
        _readySetGoPanel.StartAnimation(wave);
    }

    public void Win()
    {
        _winPopup.SetActive(true);
        _retryButton.gameObject.SetActive(true);
        _cannonPlacer.gameObject.SetActive(false);
    }

    public void Lose()
    {
        _retryButton.gameObject.SetActive(true);
        _losePopup.gameObject.SetActive(true);
        _cannonPlacer.gameObject.SetActive(false);
    }

    private void OnRetryButtonClick()
    {
        ResetValues();
        OnRetryButton?.Invoke();
    }

    private void ResetValues()
    {
        _losePopup.gameObject.SetActive(false);
        _winPopup.gameObject.SetActive(false);
        _retryButton.gameObject.SetActive(false);
    }

    private void OnReadySetGoFinishEvent()
    {
        _cannonPlacer.gameObject.SetActive(true);
    }
}
