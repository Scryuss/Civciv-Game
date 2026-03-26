using UnityEngine;
using UnityEngine.UI;

public class RottenWheatCollectibles : MonoBehaviour, ICollectible
{
    [SerializeField] WheatDesingSO _wheatDesingSO;
    
    // Artık Inspector'dan sürüklemeyeceğiz, kod kendi bulacak
    private PlayerController _playerController;
    private PlayerStateUI _playerStateUI;

    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    void Awake()
    {
        // 1. Sahnedeki Player'ı ve UI'ı otomatik olarak bul (Sihirli kısım burası)
        _playerController = FindAnyObjectByType<PlayerController>();
        _playerStateUI = FindAnyObjectByType<PlayerStateUI>();

        // 2. Referanslar bulunduktan sonra işlemlerine devam et
        _playerBoosterTransform = _playerStateUI.GetBoosterSlowTransform;
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

   public void Collect()
   {
       _playerController.SetMovementSpeed(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);

        _playerStateUI.PlayBoosterUIAnimation(_playerBoosterTransform, _playerBoosterImage, 
       _playerStateUI.GetBoosterSlowImage, _wheatDesingSO.ActiveSprite, _wheatDesingSO.PassiveSprite, 
       _wheatDesingSO.ActiveWheatSprite, _wheatDesingSO.PassiveWheatSprite, _wheatDesingSO.ResetBoostDuration);
       CameraShakeManager.Instance.ShakeCamera(0.3f);
       WheatSpawner.TriggerWheatCollected();
       UnityEngine.Object.Destroy(gameObject);
   }
}