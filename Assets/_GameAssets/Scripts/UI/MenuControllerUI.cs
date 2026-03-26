using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MaskTransitions;

public class MenuControllerUI : MonoBehaviour
{
    [Header("Buton Referansları")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _extraButton1;
    [SerializeField] private Button _extraButton2;

    [Header("Geçiş Süreleri")]
    [Tooltip("Butonların küçülerek yok olma süresi")]
    [SerializeField] private float _buttonHideDuration = 0.3f; 

    private void Awake()
    {
        if (_playButton != null)
            _playButton.onClick.AddListener(PlayGame);

        if (_quitButton != null)
            _quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        // 1. Çakışmaları önlemek için sahnedeki tüm tweenleri durdur
        DOTween.KillAll();

        // 2. Butonları etkileşime kapat
        SetButtonsInteractable(false);

        // 3. Butonların üzerindeki efekt scriptlerini kapatıyoruz
        DisableButtonEffects(_playButton);
        DisableButtonEffects(_quitButton);
        DisableButtonEffects(_extraButton1);
        DisableButtonEffects(_extraButton2);

        // 4. Asset'in Maskeli Geçiş Animasyonunu Başlat
        if (TransitionManager.Instance != null)
        {
            TransitionManager.Instance.LoadLevel("GameScene");
        }
        else
        {
            // Eğer hiyerarşide TransitionManager unutulmuşsa hata vermesin diye normal yükleme
            SceneManager.LoadScene("GameScene");
        }

        // 5. Play dahil tüm butonları küçült
        HideButton(_playButton);
        HideButton(_quitButton);
        HideButton(_extraButton1);
        HideButton(_extraButton2);
    }

    private void HideButton(Button btn)
    {
        if (btn != null)
        {
            // Önce üzerinde kalan tweenerları temizle ve sonra küçült
            btn.transform.DOKill(); 
            btn.transform.DOScale(Vector3.zero, _buttonHideDuration)
                .SetEase(Ease.InBack)
                .SetUpdate(true);
        }
    }

    // Buton üzerindeki ButtonTweenEffects scriptini bulur ve kapatır
    private void DisableButtonEffects(Button btn)
    {
        if (btn != null)
        {
            var effect = btn.GetComponent<ButtonTweenEffects>();
            if (effect != null) effect.enabled = false;
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        if (_playButton != null) _playButton.interactable = state;
        if (_quitButton != null) _quitButton.interactable = state;
        if (_extraButton1 != null) _extraButton1.interactable = state;
        if (_extraButton2 != null) _extraButton2.interactable = state;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}