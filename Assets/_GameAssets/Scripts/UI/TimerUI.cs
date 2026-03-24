using TMPro;
using UnityEngine;
using DG.Tweening;

public class TimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _timerRotateableTransform;   
    [SerializeField] private TMP_Text _timerText;

    private float _elapsedTime;
    private bool _isTimerRunning;
    
    private void Start() 
    {
        PlayRotationAnimation();
        UpdateTimerDisplay(0f); 
        
        // Zamanlayıcının otomatik olarak hemen başlaması için bu satırı ekliyoruz:
        StartTimer(); 
    }

    private void Update()
    {
        // Sadece StartTimer çağrıldıysa süreyi işletir
        if (_isTimerRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimerDisplay(_elapsedTime);
        }
    }

    private void PlayRotationAnimation()
    {
        _timerRotateableTransform.DORotate(new Vector3(0, 0, -360), 60f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }

    public void StartTimer()
    {
        _elapsedTime = 0f;
        _isTimerRunning = true;
    }

    // İhtiyacın olursa diye durdurma fonksiyonu
    public void StopTimer()
    {
        _isTimerRunning = false;
    }

    private void UpdateTimerDisplay(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60f);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60f);

        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}