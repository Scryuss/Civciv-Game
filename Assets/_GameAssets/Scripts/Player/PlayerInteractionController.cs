using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    private PlayerController _playerController;

    void Awake()
    {
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
}
