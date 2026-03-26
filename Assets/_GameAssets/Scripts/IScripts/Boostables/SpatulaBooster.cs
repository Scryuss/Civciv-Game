using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostable
{
    [Header("Spatula Booster Settings")]
    [SerializeField] private Animator _spatulaAnimator;  
    [SerializeField] private float _jumpForce;

    [Header("Camera Shake Settings")]
    [Tooltip("Spatula ile fırlatıldığında oluşacak sarsıntının şiddeti")]
    [SerializeField] private float _shakeIntensity = 1.5f;
    
    [Tooltip("Sarsıntının yönü. Geriye doğru itilme (G-Kuvveti) hissi için Z eksenine eksi değer girin.")]
    [SerializeField] private Vector3 _shakeDirection = new Vector3(0f, 0f, -1f);

    private bool _isBoosted;

    public void ApplyBoost(PlayerController playerController)
    {
        if (_isBoosted) return; // Eğer zaten boost etkisi altındaysa, tekrar uygulama

        PlayBoostAnimation();
        Rigidbody playerRigidbody = playerController.GetPlayerRigidbody();

        playerRigidbody.linearVelocity = new Vector3(playerRigidbody.linearVelocity.x, 0f, playerRigidbody.linearVelocity.z);
        playerRigidbody.AddForce(transform.forward * _jumpForce, ForceMode.Impulse);
        
        // Koda sabitlenmiş değerler yerine, yukarıda belirlediğimiz ayarlanabilir değişkenleri gönderiyoruz
        CameraShakeManager.Instance.ShakeCamera(_shakeIntensity, _shakeDirection);

        _isBoosted = true;
        Invoke(nameof(ResetBoost), 0.2f); // Boost etkisi 0.2 saniye sürer
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