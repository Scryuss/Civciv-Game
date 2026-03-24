using UnityEngine;
using UnityEngine.UI;

public class HolyWheatCollectibles : MonoBehaviour, ICollectible
{
    [SerializeField] WheatDesingSO _wheatDesingSO;
    [SerializeField] PlayerController _playerController;
    [SerializeField] private PlayerStateUI _playerStateUI;

    private RectTransform _playerBoosterTransform;
    private Image _playerBoosterImage;

    void Awake()
    {
        _playerBoosterTransform = _playerStateUI.GetBoosterJumpingTransform;
        _playerBoosterImage = _playerBoosterTransform.GetComponent<Image>();
    }

   public void Collect()
   {
       _playerController.SetJumpForce(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);

_playerStateUI.PlayBoosterUIAnimation(_playerBoosterTransform, _playerBoosterImage, 
       _playerStateUI.GetBoosterJumpingImage, _wheatDesingSO.ActiveSprite, _wheatDesingSO.PassiveSprite, 
       _wheatDesingSO.ActiveWheatSprite, _wheatDesingSO.PassiveWheatSprite, _wheatDesingSO.ResetBoostDuration);

       UnityEngine.Object.Destroy(gameObject);
   }
}
