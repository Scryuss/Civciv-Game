using System;
using System.Collections; // Coroutine (Gecikme) için gerekli
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct WheatSpawnChance
{
    public GameObject WheatPrefab;
    [Range(0f, 100f)]
    public float SpawnWeight; 
}

public class WheatSpawner : MonoBehaviour
{
    // YENİ: Buğdayların toplandığında haber vereceği radyo kanalı
    public static event Action OnWheatCollected; 

    [Header("Wheat Spawn Settings")]
    [SerializeField] private WheatSpawnChance[] _wheatTypes; 
    [SerializeField] private int _wheatsToSpawn = 5; 
    [SerializeField] private float _spawnYOffset = 2f; 

    // YENİ: Bekleme süresi ayarı
    [Header("Respawn Settings")]
    [Tooltip("Buğday toplandıktan kaç saniye sonra yenisi çıksın?")]
    [SerializeField] private float _respawnDelay = 10f;

    private Transform[] _spawnPoints; 

    private void Awake()
    {
        _spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            _spawnPoints[i] = transform.GetChild(i);
        }
    }

    // Olayı dinlemeye başla
    private void OnEnable()
    {
        OnWheatCollected += HandleWheatCollected;
    }

    // Olayı dinlemeyi bırak (Hafıza sızıntısını önler)
    private void OnDisable()
    {
        OnWheatCollected -= HandleWheatCollected;
    }

    private void Start()
    {
        // Oyun başlarken ilk 5 tanesini (veya belirlediğin sayı kadarını) doğur
        for (int i = 0; i < _wheatsToSpawn; i++)
        {
            SpawnSingleWheat();
        }
    }

    // Bir buğday toplandığında burası tetiklenir
    private void HandleWheatCollected()
    {
        // 10 saniyelik üretim sürecini başlat
        StartCoroutine(RespawnRoutine());
    }

    // Arka planda 10 saniye sayan sayaç
    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_respawnDelay); // Belirlenen süre kadar bekle
        SpawnSingleWheat(); // Süre bitince 1 tane yeni buğday üret
    }

    // Tek bir buğday üretme fonksiyonu (Sihirli taktik burada)
    private void SpawnSingleWheat()
    {
        // 1. Sadece "BOŞ" olan noktaları bul
        List<Transform> availablePoints = new List<Transform>();
        foreach (Transform point in _spawnPoints)
        {
            // Eğer noktanın içinde obje yoksa (çocuk sayısı 0 ise) orası boştur
            if (point.childCount == 0)
            {
                availablePoints.Add(point);
            }
        }

        // Eğer haritada hiç boş yer kalmadıysa (çok zor ihtimal) iptal et
        if (availablePoints.Count == 0) return;

        // 2. Boş noktalardan rastgele birini seç
        int randomIndex = Random.Range(0, availablePoints.Count);
        Transform selectedPoint = availablePoints[randomIndex];

        // 3. İhtimallere göre rastgele bir buğday türü seç
        GameObject prefabToSpawn = GetRandomWheatByChance();

        if (prefabToSpawn != null)
        {
            Vector3 finalSpawnPosition = selectedPoint.position + new Vector3(0f, _spawnYOffset, 0f);

            // 4. ÇOK KRİTİK: selectedPoint'i Instantiate komutunun sonuna ekledik.
            // Bu sayede yeni doğan buğday o noktanın İÇİNE (child) girecek. Nokta "dolu" sayılacak.
            GameObject spawnedWheat = Instantiate(prefabToSpawn, finalSpawnPosition, selectedPoint.rotation, selectedPoint);
            spawnedWheat.SetActive(true);
        }
    }

    private GameObject GetRandomWheatByChance()
    {
        float totalWeight = 0f;
        foreach (var wheatType in _wheatTypes)
        {
            totalWeight += wheatType.SpawnWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        foreach (var wheatType in _wheatTypes)
        {
            randomValue -= wheatType.SpawnWeight;
            if (randomValue <= 0f)
            {
                return wheatType.WheatPrefab;
            }
        }
        return null; 
    }

    // YENİ: Dışarıdaki scriptlerin buğday toplandığını Spawner'a bildirmesi için köprü metot
    public static void TriggerWheatCollected()
    {
        OnWheatCollected?.Invoke();
    }
}