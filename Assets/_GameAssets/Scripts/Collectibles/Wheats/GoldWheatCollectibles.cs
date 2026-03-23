using UnityEngine;

public class GoldWheatCollectibles : MonoBehaviour
{
   [SerializeField] PlayerController _playerController;
   [SerializeField] float _movementSpeedIncrease;
   [SerializeField] float _ResetBoostTime;

   public void Collect()
   {
       _playerController.SetMovementSpeed(_movementSpeedIncrease, _ResetBoostTime);
   }
}
