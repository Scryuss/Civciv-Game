using TMPro;
using UnityEngine;
using DG.Tweening;
using System;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _timerRotateableTransform;   
    [SerializeField] private TMP_Text _timerText;

    private float _elapsedTime;
    private bool _isTimerRunning;
    private Tween _rotationTween;
    
    private void Start() 
    {
        PlayRotationAnimation();
        UpdateTimerDisplay(0f); 
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged; 
        
        StartTimer(); 
    }

    private void GameManager_OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
                PauseTimer();
                break;
            case GameState.Resume:
                ResumeTimer();
                break;
        } 
    }

    private void Update()
    {
        // Sadece StartTimer veya ResumeTimer çağrıldıysa süreyi işletir
        if (_isTimerRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(_elapsedTime);
        }
    }

    private void PlayRotationAnimation()
    {
        _rotationTween = _timerRotateableTransform.DORotate(new Vector3(0, 0, -360), 60f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isTimerRunning = true;
    }

    private void PauseTimer()
    {
        _isTimerRunning = false; // Update içindeki sayacı durdurur
        _rotationTween.Pause(); // Animasyonu durdur
    }

    private void ResumeTimer()
    {
        if (!_isTimerRunning)
        {
            _isTimerRunning = true; // Update içindeki sayacı kaldığı yerden başlatır
            _rotationTween.Play(); // Animasyonu devam ettir
        }
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}