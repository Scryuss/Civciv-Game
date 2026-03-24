using UnityEngine;

public class HolyWheatCollectibles : MonoBehaviour, ICollectible
{
    [SerializeField] WheatDesingSO _wheatDesingSO;
    [SerializeField] PlayerController _playerController;

   public void Collect()
   {
       _playerController.SetJumpForce(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);
       UnityEngine.Object.Destroy(gameObject);
   }
}
