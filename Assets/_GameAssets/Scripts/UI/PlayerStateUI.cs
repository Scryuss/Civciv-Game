using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [Header("Player State UI")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private RectTransform _playerWalkingTransform;
    [SerializeField] private RectTransform _playerSlidingTransform;
    [SerializeField] private RectTransform _boosterJumpingTransform;
    [SerializeField] private RectTransform _boosterSpeedTransform;
    [SerializeField] private RectTransform _boosterSlowTransform;

    [Header("Images")]
    [SerializeField] private Image _boosterJumpingImage;
    [SerializeField] private Image _boosterSpeedImage;
    [SerializeField] private Image _boosterSlowImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _playerWalkingActiveSprite;
    [SerializeField] private Sprite _playerWalkingPassiveSprite;
    [SerializeField] private Sprite _playerSlidingActiveSprite;
    [SerializeField] private Sprite _playerSlidingPassiveSprite;

    [Header("Animation Settings")]
    [SerializeField] private float _moveDuration;
    [SerializeField] private Ease _moveEase;

    private Coroutine _jumpingCoroutine;
    private Coroutine _speedCoroutine;
    private Coroutine _slowCoroutine;

    public RectTransform GetBoosterJumpingTransform => _boosterJumpingTransform;
    public RectTransform GetBoosterSpeedTransform => _boosterSpeedTransform;
    public RectTransform GetBoosterSlowTransform => _boosterSlowTransform;

    public Image GetBoosterJumpingImage => _boosterJumpingImage;
    public Image GetBoosterSpeedImage => _boosterSpeedImage;
    public Image GetBoosterSlowImage => _boosterSlowImage;

    private Image _playerWalkingImage;
    private Image _playerSlidingImage;

    void Awake()
    {
        _playerWalkingImage = _playerWalkingTransform.GetComponent<Image>();
        _playerSlidingImage = _playerSlidingTransform.GetComponent<Image>();
    }

    void Start()
    {
        _playerController.OnPlayerStateChanged += PlayerController_OnPlayerStateChanged;
        SetStateUserInterfaces(_playerWalkingActiveSprite, _playerSlidingPassiveSprite,
                     _playerWalkingTransform, _playerSlidingTransform);   
    }

    private void PlayerController_OnPlayerStateChanged(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Move:
            case PlayerState.Idle:
                    SetStateUserInterfaces(_playerWalkingActiveSprite, _playerSlidingPassiveSprite,
                     _playerWalkingTransform, _playerSlidingTransform);
                break;
            case PlayerState.Slide:
            case PlayerState.SlideIdle:
                    SetStateUserInterfaces(_playerWalkingPassiveSprite, _playerSlidingActiveSprite,
                     _playerSlidingTransform, _playerWalkingTransform);
                break;
        }
    }

    private void SetStateUserInterfaces(Sprite playerWalkingSprite, Sprite playerSlidingSprite,
    RectTransform activeTransform, RectTransform passiveTransform)
    {
        _playerWalkingImage.sprite = playerWalkingSprite;
        _playerSlidingImage.sprite = playerSlidingSprite;

        activeTransform.DOAnchorPosX(80f, _moveDuration).SetEase(_moveEase);
        passiveTransform.DOAnchorPosX(20f, _moveDuration).SetEase(_moveEase);
    }

    private IEnumerator SetBoosterUserInterface(RectTransform ActiveTransform, Image BoosterImage, Image WheatImage, Sprite BoosterActiveSprite,
     Sprite BoosterPassiveSprite, Sprite ActiveWheatSprite, Sprite PassiveWheatSprite, float duration)
    {
        BoosterImage.sprite = BoosterActiveSprite;
        WheatImage.sprite = ActiveWheatSprite;
        ActiveTransform.DOAnchorPosX(-80f, _moveDuration).SetEase(_moveEase);
        
        yield return new WaitForSeconds(duration);

        BoosterImage.sprite = BoosterPassiveSprite;
        WheatImage.sprite = PassiveWheatSprite;
        ActiveTransform.DOAnchorPosX(0f, _moveDuration).SetEase(_moveEase);
    }

    public void PlayBoosterUIAnimation(RectTransform activeTransform, Image boosterImage, Image wheatImage, Sprite boosterActiveSprite, Sprite boosterPassiveSprite, Sprite activeWheatSprite, Sprite passiveWheatSprite, float duration)
{
    // Hangi panelin animasyonuysa, onun mevcut Coroutine'ini durdur
    if (activeTransform == _boosterJumpingTransform)
    {
        if (_jumpingCoroutine != null) StopCoroutine(_jumpingCoroutine);
        _jumpingCoroutine = StartCoroutine(SetBoosterUserInterface(activeTransform, boosterImage, wheatImage, boosterActiveSprite, boosterPassiveSprite, activeWheatSprite, passiveWheatSprite, duration));
    }
    else if (activeTransform == _boosterSpeedTransform)
    {
        if (_speedCoroutine != null) StopCoroutine(_speedCoroutine);
        _speedCoroutine = StartCoroutine(SetBoosterUserInterface(activeTransform, boosterImage, wheatImage, boosterActiveSprite, boosterPassiveSprite, activeWheatSprite, passiveWheatSprite, duration));
    }
    else if (activeTransform == _boosterSlowTransform)
    {
        if (_slowCoroutine != null) StopCoroutine(_slowCoroutine);
        _slowCoroutine = StartCoroutine(SetBoosterUserInterface(activeTransform, boosterImage, wheatImage, boosterActiveSprite, boosterPassiveSprite, activeWheatSprite, passiveWheatSprite, duration));
    }
}
}
