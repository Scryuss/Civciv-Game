using TMPro;
using UnityEngine;
using DG.Tweening;

public class EggCounterUI : MonoBehaviour
{
    [Header("Referanslar")]
    [SerializeField] private TMP_Text _eggText;
    
    [Header("Tamamlanma Rengi")]
    [SerializeField] private Color _completionColor = new Color(0.2f, 1f, 0.2f); // Parlak yeşil

    [Header("Sonsuz Süzülme Ayarları (Hover)")]
    [Tooltip("Yazının sağa sola/yukarı aşağı süzülme süresi. Değer büyüdükçe hareket yavaşlar.")]
    [SerializeField] private float _hoverDuration = 2f;
    
    [Tooltip("Süzülme hareketinin ne kadar geniş bir alana yayılacağı (Şiddeti).")]
    [SerializeField] private float _hoverStrength = 5f;
    
    [Tooltip("Hareketin titreme detayı. Süzülme hissi için düşük kalmalıdır (Örn: 5-10 arası).")]
    [SerializeField] private int _hoverVibrato = 8;

    [Header("Sonsuz Nabız Ayarları (Breathing)")]
    [Tooltip("Yazının nefes alırken ulaşacağı maksimum büyüklük (Ölçek).")]
    [SerializeField] private float _pulseTargetScale = 1.3f;
    
    [Tooltip("Büyüyüp küçülme işleminin ne kadar süreceği. Değer büyüdükçe nabız yavaşlar.")]
    [SerializeField] private float _pulseDuration = 1.5f;


    private Color _originalColor;

    private void Awake()
    {
        _originalColor = _eggText.color;
    }

    private void OnEnable()
    {
        GameManager.OnEggCountUpdated += UpdateEggText;
    }

    // Scripts/UI/EggCounterUI.cs içindeki OnDisable metodunu bu şekilde güncelle
private void OnDisable()
{
    GameManager.OnEggCountUpdated -= UpdateEggText;

    // Obje devre dışı kalırken (sahne değişirken) üzerindeki animasyonları öldür
    if (_eggText != null)
    {
        _eggText.transform.DOKill();
    }
}

    private void UpdateEggText(int currentCount, int targetCount)
    {
        _eggText.text = $"{currentCount}/{targetCount}";

        // Önceki animasyonları temizle ve hizayı sıfırla
        _eggText.transform.DOKill();
        _eggText.transform.localScale = Vector3.one;
        _eggText.transform.localRotation = Quaternion.identity;

        if (currentCount == 0) return;

        if (currentCount == targetCount)
        {
            PlayVigorousCompletionAnimation();
        }
        else 
        {
            _eggText.color = _originalColor;
            _eggText.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 10, 1f);
        }
    }

    private void PlayVigorousCompletionAnimation()
    {
        // Renk parlaması: Önce anlık bembeyaz yap, sonra parlak yeşile geçiş yap
        _eggText.color = Color.white;
        _eggText.DOColor(_completionColor, 0.3f);

        Sequence completeSeq = DOTween.Sequence();
        
        // Adım 1: Hızlıca şişme
        completeSeq.Append(_eggText.transform.DOScale(1.5f, 0.15f).SetEase(Ease.OutBack)); 
        
        // Adım 2: Coşkulu sallanma ve zıplama
        completeSeq.Join(_eggText.transform.DOPunchRotation(Vector3.forward * 15f, 0.35f, 15, 1f));
        completeSeq.Join(_eggText.transform.DOPunchScale(Vector3.one * 0.5f, 0.35f, 10, 1f)); 

        // Adım 3: Normale dönme, biraz büyük kal (1.2f)
        completeSeq.Append(_eggText.transform.DOScale(1.2f, 0.25f).SetEase(Ease.OutBounce));

        // KUTLAMA BİTTİĞİNDE INSPECTOR'DAKİ DEĞERLERLE SONSUZ DÖNGÜ BAŞLASIN
        completeSeq.OnComplete(() => 
        {
            // --- 1. HAREKET: ORGANİK SÜZÜLME (POZİSYON) ---
            _eggText.transform.DOShakePosition(_hoverDuration, _hoverStrength, _hoverVibrato, 90, false, false)
                .SetRelative(true)
                .SetLoops(-1);

            // --- 2. HAREKET: HAFİF BÜYÜYÜP KÜÇÜLME (ÖLÇEK) ---
            _eggText.transform.DOScale(_pulseTargetScale, _pulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutQuad); 
        });
    }
}