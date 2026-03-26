using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MaskTransitions; // Asset'in kütüphanesini ekledik

public class SettingsUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _settingsPopupObject;
    [SerializeField] private GameObject _blackBackgroundObject;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _buttonHideDuration = 0.3f; // Butonların kaybolma süresi

    [Header("Ease Settings")]
    [SerializeField] private Ease _openEase = Ease.OutBack;
    [SerializeField] private Ease _closeEase = Ease.InBack;

    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _restartButton;

    private Image _blackBackgroundImage;
    private bool _isSettingsOpen = false; 

    void Awake()
    {
        _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
        _settingsPopupObject.transform.localScale = Vector3.zero;

        _settingsButton.onClick.AddListener(ToggleSettings);
        _resumeButton.onClick.AddListener(ToggleSettings);
        _restartButton.onClick.AddListener(RestartGame);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
{
    // EKLENEN KISIM: Eğer oyun durumu "GameOver" ise alt satırlara inmeden işlemi iptal et
    if (_gameManager.GetCurrentGameState() == GameState.GameOver)
    {
        return; 
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
        ToggleSettings();
    }
}

    private void ToggleSettings()
    {
        _isSettingsOpen = !_isSettingsOpen; 

        if (_isSettingsOpen)
            OpenSettingsMenu();
        else
            CloseSettingsMenu();
    }

    private void OpenSettingsMenu()
    {
        _gameManager.ChangeGameState(GameState.Paused); 

        _blackBackgroundObject.SetActive(true);
        _settingsPopupObject.SetActive(true);
        
        _blackBackgroundImage.DOKill();
        _settingsPopupObject.transform.DOKill();

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(_openEase).SetUpdate(true);
        _settingsPopupObject.transform.DOScale(1.5f, _animationDuration).SetEase(_openEase).SetUpdate(true);
    }

    private void CloseSettingsMenu()
    {
        _blackBackgroundImage.DOKill();
        _settingsPopupObject.transform.DOKill();

        _blackBackgroundImage.DOFade(0f, _animationDuration).SetEase(_closeEase).SetUpdate(true);
        _settingsPopupObject.transform.DOScale(0f, _animationDuration).SetEase(_closeEase).SetUpdate(true)
            .OnComplete(() => 
            {
                _gameManager.ChangeGameState(GameState.Resume); 
                _blackBackgroundObject.SetActive(false);
                _settingsPopupObject.SetActive(false);
            });
    }

    private void RestartGame()
    {
        Time.timeScale = 1f; // ÖNEMLİ: Zamanı açmazsak TransitionManager animasyonu donar
        DOTween.KillAll();

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadLevel(SceneManager.GetActiveScene().name);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        HideAllButtons();
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        DOTween.KillAll(); 

        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadLevel("MenuScene"); 
        else
            SceneManager.LoadScene("MenuScene"); 

        HideAllButtons();
    }

    // Ekrandaki tüm butonları şıkça küçülterek yok eder
    private void HideAllButtons()
    {
        HideButton(_settingsButton);
        HideButton(_musicButton);
        HideButton(_soundButton);
        HideButton(_resumeButton);
        HideButton(_mainMenuButton);
        HideButton(_restartButton);
    }

    private void HideButton(Button btn)
    {
        if (btn != null)
        {
            // Fare etkileşimi animasyonu bozmasın diye ButtonTweenEffects'i kapat
            var effect = btn.GetComponent<ButtonTweenEffects>();
            if (effect != null) effect.enabled = false;

            btn.interactable = false;
            btn.transform.DOKill();
            btn.transform.DOScale(Vector3.zero, _buttonHideDuration).SetEase(Ease.InBack).SetUpdate(true);
        }
    }
}