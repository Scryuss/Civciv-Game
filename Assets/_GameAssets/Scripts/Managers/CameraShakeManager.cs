using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }

    private CinemachineImpulseSource _impulseSource;

    [Header("Genel Sarsıntı Ayarı")]
    [Tooltip("Tüm sarsıntıların gücünü tek bir yerden kısmak veya artırmak için çarpan.")]
    [SerializeField] private float _globalShakeMultiplier = 0.3f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // 1. RASTGELE YÖNLÜ SARSINTI (Hasar ve Buğdayların mevcut kodunu bozmamak için)
    public void ShakeCamera(float intensity)
    {
        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f), 
            Random.Range(-1f, 1f), 
            Random.Range(-1f, 1f)
        ).normalized;

        // Rastgele yönü bulup asıl sarsıntı metoduna gönderiyoruz
        ShakeCamera(intensity, randomDirection);
    }

    // 2. YENİ: BELİRLİ BİR YÖNE SARSINTI (Spatula gibi özel yön isteyenler için)
    public void ShakeCamera(float intensity, Vector3 direction)
    {
        if (_impulseSource != null)
        {
            float finalIntensity = intensity * _globalShakeMultiplier;
            // Gelen yönü (direction) normalize edip gücümüzle çarpıyoruz
            _impulseSource.GenerateImpulse(direction.normalized * finalIntensity);
        }
    }
}