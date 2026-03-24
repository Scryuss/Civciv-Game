using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{
    [Header("Spatula Booster Settings")]
    [SerializeField] private Animator _spatulaAnimator;  
    [SerializeField] private float _jumpForce;

    private bool _isBoosted;
    public void ApplyBoost(PlayerController playerController)
    {
        if (_isBoosted) return; // Eğer zaten boost etkisi altındaysa, tekrar uygulama

        PlayBoostAnimation();
        Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();

        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
        playerRigidbody.AddForce(transform.forward * _jumpForce, ForceMode.Impulse);
        _isBoosted = true;
        Invoke(nameof(ResetBoost), 0.2f); // Boost etkisi 2 saniye sürer
    }

    private void ResetBoost()
    {
        _isBoosted = false;
    } 

    private void PlayBoostAnimation()
    {
        _spatulaAnimator.SetTrigger("IsSpatulaJumping");
    }
}
