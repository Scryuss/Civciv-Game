using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Health UI Settings")]
    [SerializeField] private Image[] _playerHealthImages;
    [SerializeField] private Sprite _playerHealthySprite;
    [SerializeField] private Sprite _playerDamagedSprite;
    [SerializeField] private float _scaleDuration;

    private RectTransform[] _playerHealthTransforms;

    void Awake()
    {
        _playerHealthTransforms = new RectTransform[_playerHealthImages.Length];
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            _playerHealthTransforms[i] = _playerHealthImages[i].gameObject.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AnimateDamage(); // Example: Animate damage for 1 health point
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AnimateDamageForAll(); // Example: Animate damage for all health points
        }
    }

    public void AnimateDamage()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            if(_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimatedDamageSprites(_playerHealthImages[i], _playerHealthTransforms[i]);
                break;
            }
            
        }
    }

    public void AnimateDamageForAll()
    {
        for (int i = 0; i < _playerHealthImages.Length; i++)
        {
            if(_playerHealthImages[i].sprite == _playerHealthySprite)
            {
                AnimatedDamageSprites(_playerHealthImages[i], _playerHealthTransforms[i]);
            }
        }
    }

    private void AnimatedDamageSprites(Image activeImage, RectTransform activeImageTransform)
    {
        activeImageTransform.DOScale(0f, _scaleDuration).SetEase(Ease.InBack).OnComplete(() =>
        {
            activeImage.sprite = _playerDamagedSprite;
            activeImageTransform.DOScale(1f, _scaleDuration).SetEase(Ease.OutBack);
        });
    }
}
