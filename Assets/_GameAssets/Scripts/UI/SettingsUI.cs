using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _settingsPopupObject;
    [SerializeField] private GameObject _blackBackgroundObject;
    
    [Header("Animation Settings")]
    [SerializeField] private float _animationDuration;

    [Header("Ease Settings")]
    [Tooltip("Menü açılış animasyonunun hız eğrisi (Örn: OutBack)")]
    [SerializeField] private Ease _openEase = Ease.OutBack;
    
    [Tooltip("Menü kapanış animasyonunun hız eğrisi (Örn: InBack)")]
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
        // YENİ: Restart butonuna tıklandığında RestartGame metodunu çalıştır
        _restartButton.onClick.AddListener(RestartGame);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings();
        }
    }

    private void ToggleSettings()
    {
        _isSettingsOpen = !_isSettingsOpen; 

        if (_isSettingsOpen)
        {
            OpenSettingsMenu();
        }
        else
        {
            CloseSettingsMenu();
        }
    }

    private void OpenSettingsMenu()
    {
        _gameManager.ChangeGameState(GameState.Paused); 

        _blackBackgroundObject.SetActive(true);
        _settingsPopupObject.SetActive(true);
        
        // YENİ: Varsa devam eden eski animasyonları iptal et (Spam koruması)
        _blackBackgroundImage.DOKill();
        _settingsPopupObject.transform.DOKill();

        // Animasyonları başlat
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
                _gameManager.ChangeGameState(GameState.Resume); // Menü kapandıktan sonra Resume durumuna geç
                _blackBackgroundObject.SetActive(false);
                _settingsPopupObject.SetActive(false);
                
            });
    }

    private void RestartGame()
        {
            // 1. ÇOK ÖNEMLİ: Zamanı tekrar akmaya başlat (Yoksa yeni sahne donuk yüklenir)
            Time.timeScale = 1f;

            // 2. Sahne yüklenirken arkada devam eden eski UI animasyonlarını öldür (Hata almamak için)
            DOTween.KillAll();

            // 3. Şu an aktif olan sahneyi (GameScene) baştan yükle
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void GoToMainMenu()
{
    Time.timeScale = 1f; // Zamanı normale döndür
    DOTween.KillAll(); // Animasyonları temizle
    SceneManager.LoadScene("MenuScene"); // "MainMenuScene" yerine kendi ana menü sahnenin adını yazmalısın
}
}