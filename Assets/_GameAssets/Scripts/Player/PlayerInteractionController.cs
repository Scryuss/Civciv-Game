using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private Transform _playerVisualTransform;
    private Rigidbody _playerRigidbody;
    private PlayerController _playerController;

    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ICollectible>(out var collectible))
            {
                collectible.Collect();
            }
    }
    
    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.TryGetComponent<IBoostable>(out var boostable))
        {
            boostable.ApplyBoost(_playerController);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.GiveDamage(_playerRigidbody, _playerVisualTransform);
        }
    }
}
