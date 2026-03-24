using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{   
    [Header("Camera Settings")]
  [SerializeField] Transform _playerTransform;
  [SerializeField] Transform _orientationTransform;
  [SerializeField] Transform _playerVisualTransform;

    [Header("Camera Rotation")]
  [SerializeField] float _rotationSpeed;

    void Update()
    {
      if (GameManager.Instance.GetCurrentGameState() != GameState.Playing
          && GameManager.Instance.GetCurrentGameState() != GameState.Resume)
          {
              return; // Oyun durumu Playing veya Resume değilse, kamera hareketini engelle
          }

        Vector3 viewDirection = _playerVisualTransform.position - new Vector3(transform.position.x, _playerVisualTransform.position.y, transform.position.z);
        _orientationTransform.forward = viewDirection.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = _orientationTransform.forward * verticalInput + _orientationTransform.right * horizontalInput;

        if (inputDirection != Vector3.zero)
        {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, inputDirection.normalized, _rotationSpeed * Time.deltaTime);
        }  
    }
}
