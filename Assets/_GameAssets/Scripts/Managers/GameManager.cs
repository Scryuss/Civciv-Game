using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // UI scriptinin dinleyeceği yeni anons kanalımız (MevcutSayı ve HedefSayı'yı gönderir)
    public static event Action<int, int> OnEggCountUpdated; 
    public static GameManager Instance { get; private set; } // Singleton örneği

    public event Action<GameState> OnGameStateChanged; // Oyun durumunu dinlemek isteyen diğer sistemler için anons kanalı

    [SerializeField] private WinLoseUI _winLoseUI; // Oyun kazanıldığında veya kaybedildiğinde gösterilecek UI referansı

    [Header("Level Settings")]
    [SerializeField] private int _targetEggCount = 5; 
    [SerializeField] private float _delay;

    private GameState _currentGameState;
    
    private int _currentEggCount = 0;

    private void Awake()
    {
        // Eğer sahnede henüz bir GameManager yoksa, kendini Instance olarak ata
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Yanlışlıkla sahneye ikinci bir GameManager eklendiyse onu yok et (Güvenlik)
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Oyun başlar başlamaz UI'ın "0/5" olarak güncellenmesi için ilk yayını yapıyoruz
        OnEggCountUpdated?.Invoke(_currentEggCount, _targetEggCount);

        HealthManager.Instance.OnPlayerDeath += HealthManager_OnplayerDeath;
    }

    private void HealthManager_OnplayerDeath()
    {
        StartCoroutine(OnGameOver());
    }

    private void OnEnable()
    {
        EggCollectable.OnEggCollected += HandleEggCollected;
        ChangeGameState(GameState.Playing); // Oyun başladığında durumu Playing olarak ayarlıyoruz
    }

    private void OnDisable()
    {
        EggCollectable.OnEggCollected -= HandleEggCollected;
    }

    private void HandleEggCollected()
    {
        _currentEggCount++;
        
        // Skor arttı! UI'a yeni sayıları haber ver
        OnEggCountUpdated?.Invoke(_currentEggCount, _targetEggCount);

        if (_currentEggCount >= _targetEggCount)
        {
            GameWin();
        }
    }

    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState); 
        _currentGameState = gameState;
        Debug.Log($"Game State changed to: {gameState}");

        // ZAMANI YÖNETME İŞLEMİ
        if (gameState == GameState.Paused)
        {
            Time.timeScale = 0f; // Zamanı tamamen dondur (Fizik, NavMesh, Animasyonlar durur)
        }
        else if (gameState == GameState.Playing || gameState == GameState.Resume)
        {
            Time.timeScale = 1f; // Zamanı normal akışına geri döndür
        }
    }

    private void GameWin()
    {
        // WİN
        ChangeGameState(GameState.GameOver);
        _winLoseUI.OnGameWin(); // Kazanma durumunu UI'a bildir
    }

    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(_delay);
        ChangeGameState(GameState.GameOver);
        _winLoseUI.OnGameLose();
    }
}