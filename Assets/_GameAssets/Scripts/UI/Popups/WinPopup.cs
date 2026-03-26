using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class WinPopup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private TMP_Text _timerText;

    private void OnEnable() 
    {
        _timerText.text = _timerUI.GetFinalTime(); // TimerUI'dan final zamanı alıp ekrana yazdırıyoruz
        _oneMoreButton.onClick.AddListener(OnOneMoreButtonClicked);
        _mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnOneMoreButtonClicked()
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
