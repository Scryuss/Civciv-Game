using UnityEngine;

public class RottenWheatCollectibles : MonoBehaviour, ICollectible
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] float _movementSpeedDecrease;
    [SerializeField] float _ResetBoostTime;

   public void Collect()
   {
       _playerController.SetMovementSpeed(_movementSpeedDecrease, _ResetBoostTime);
       UnityEngine.Object.Destroy(gameObject);
   }
}
