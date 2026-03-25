using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;      
    [SerializeField] private Transform _cameraPivot;           
    [SerializeField] private Transform _orientationTransform;  
    [SerializeField] private Transform _playerVisualTransform; 

    [Header("Mouse Settings")]
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _rotationSpeed = 10f; 
    
    [Header("Vertical Look Limits (Pitch)")]
    [Tooltip("Yukarı bakma sınırı (Genelde eksi değer verilir, örn: -35)")]
    [SerializeField] private float _minPitch = -30f;
    
    [Tooltip("Aşağı bakma sınırı (Genelde artı değer verilir, örn: 80)")]
    [SerializeField] private float _maxPitch = 60f;

    private float _rotationX; 
    private float _rotationY; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (GameManager.Instance.GetCurrentGameState() != GameState.Playing &&
            GameManager.Instance.GetCurrentGameState() != GameState.Resume)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        _rotationY += mouseX;
        _rotationX -= mouseY;

        // ARTIK BURADAKİ DEĞERLERİ INSPECTOR'DAN ALIYORUZ
        _rotationX = Mathf.Clamp(_rotationX, _minPitch, _maxPitch); 

        _cameraPivot.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        _orientationTransform.rotation = Quaternion.Euler(0, _rotationY, 0);

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = _orientationTransform.forward * verticalInput + _orientationTransform.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, inputDir.normalized, _rotationSpeed * Time.deltaTime);
        }
    }
}