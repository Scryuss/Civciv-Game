using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // UI scriptinin dinleyeceği yeni anons kanalımız (MevcutSayı ve HedefSayı'yı gönderir)
    public static event Action<int, int> OnEggCountUpdated; 

    [Header("Level Settings")]
    [SerializeField] private int _targetEggCount = 5; 
    
    private int _currentEggCount = 0;

    private void Start()
    {
        // Oyun başlar başlamaz UI'ın "0/5" olarak güncellenmesi için ilk yayını yapıyoruz
        OnEggCountUpdated?.Invoke(_currentEggCount, _targetEggCount);
    }

    private void OnEnable()
    {
        EggCollectable.OnEggCollected += HandleEggCollected;
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

    private void GameWin()
    {
        Debug.Log("Game Win!");
    }
}