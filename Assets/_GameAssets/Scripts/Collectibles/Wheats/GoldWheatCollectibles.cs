using UnityEngine;

public class GoldWheatCollectibles : MonoBehaviour, ICollectible
{
   [SerializeField] PlayerController _playerController;
   [SerializeField] float _movementSpeedIncrease;
   [SerializeField] float _ResetBoostTime;

   public void Collect()
   {
       _playerController.SetMovementSpeed(_movementSpeedIncrease, _ResetBoostTime);
       UnityEngine.Object.Destroy(gameObject);
   }
}
