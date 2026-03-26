using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonTweenEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("Hover Ayarları")]
    [SerializeField] private float _hoverScale = 1.1f;
    [SerializeField] private float _hoverDuration = 0.2f;
    [SerializeField] private Ease _hoverEase = Ease.OutBack;

    [Header("Click Ayarları")]
    [SerializeField] private float _clickScale = 0.9f;
    [SerializeField] private float _clickDuration = 0.1f;
    // YENİ: Tıklama animasyonu için Ease seçeneği
    [SerializeField] private Ease _clickEase = Ease.OutQuad; 

    private Vector3 _originalScale;

    void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_originalScale * _hoverScale, _hoverDuration).SetEase(_hoverEase).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_originalScale, _hoverDuration).SetEase(Ease.OutQuad).SetUpdate(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Sequence clickSeq = DOTween.Sequence();
        
        // İlk adımda buton küçülürken (basılma hissi) senin seçtiğin _clickEase uygulanır
        clickSeq.Append(transform.DOScale(_originalScale * _clickScale, _clickDuration).SetEase(_clickEase));
        
        // İkinci adımda buton tekrar hover ölçeğine döner (yaylanma hissi için OutBack sabit kalabilir veya bunu da değişken yapabilirsin)
        clickSeq.Append(transform.DOScale(_originalScale * _hoverScale, _clickDuration).SetEase(Ease.OutBack));
        
        clickSeq.SetUpdate(true); 
    }
    
    private void OnDisable() 
    {
        transform.DOKill();
        transform.localScale = _originalScale;
    }
}