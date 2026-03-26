using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MaskTransitions; // Eklendi

public class LosePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;

    [Header("Transition Settings")]
    [SerializeField] private float _buttonHideDuration = 0.3f; 

    private void OnEnable() 
    {
        _timerText.text = _timerUI.GetFinalTime(); 
        _tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnTryAgainButtonClicked()
    {
        Time.timeScale = 1f;
        DOTween.KillAll();
        
        if (TransitionManager.Instance != null)
            TransitionManager.Instance.LoadLevel("GameScene");
        else
            SceneManager.LoadScene("GameScene"); 

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

    private void HideAllButtons()
    {
        HideButton(_tryAgainButton);
        HideButton(_mainMenuButton);
    }

    private void HideButton(Button btn)
    {
        if (btn != null)
        {
            var effect = btn.GetComponent<ButtonTweenEffects>();
            if (effect != null) effect.enabled = false;

            btn.interactable = false;
            btn.transform.DOKill();
            btn.transform.DOScale(Vector3.zero, _buttonHideDuration).SetEase(Ease.InBack).SetUpdate(true);
        }
    }
}