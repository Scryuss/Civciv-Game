using UnityEngine;

public class RottenWheatCollectibles : MonoBehaviour, ICollectible
{
    [SerializeField] WheatDesingSO _wheatDesingSO;
    [SerializeField] PlayerController _playerController;

   public void Collect()
   {
       _playerController.SetMovementSpeed(_wheatDesingSO.IncreaseDecreaseMultiplier, _wheatDesingSO.ResetBoostDuration);
       UnityEngine.Object.Destroy(gameObject);
   }
}
