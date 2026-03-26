using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;

    private void OnEnable() 
    {
        _timerText.text = _timerUI.GetFinalTime(); // TimerUI'dan final zamanı alıp ekrana yazdırıyoruz
        _tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnTryAgainButtonClicked()
    {
        DOTween.KillAll();
        SceneManager.LoadScene("GameScene"); 
    }

    private void GoToMainMenu()
{
    DOTween.KillAll(); //
    SceneManager.LoadScene("MenuScene"); 
}
}
