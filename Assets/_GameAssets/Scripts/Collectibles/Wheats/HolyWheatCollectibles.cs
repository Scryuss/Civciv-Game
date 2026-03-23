using UnityEngine;

public class HolyWheatCollectibles : MonoBehaviour
{
    [SerializeField] PlayerController _playerController;
    [SerializeField] float _jumpForceIncrease;
    [SerializeField] float _ResetBoostTime;

   public void Collect()
   {
       _playerController.SetJumpForce(_jumpForceIncrease, _ResetBoostTime);
   }
}
