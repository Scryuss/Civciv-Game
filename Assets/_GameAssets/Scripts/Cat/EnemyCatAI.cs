using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(NavMeshAgent), typeof(CatStateController))]
public class EnemyCatAI : MonoBehaviour
{
    [Header("Takılma Kontrolü")]
    public float maxTakilmaSuresi = 2f; // Kaç saniye kıpırdayamazsa "takıldı" sayalım?
    private float takilmaZamanlayici = 0f;

    [Header("Devriye Ayarları")]
    public float gezmeYaricapi = 15f;      
    public float gezmeAraligi = 4f;     
    public float devriyeHizi = 4f;

    [Header("Kovalama Ayarları")]
    public float kovalamaHizi = 9f;
    public float ivmelenme = 30f;
    public float donusHizi = 720f;
    public float saldiriMesafesi = 1.8f;

    [Header("Saldırı Zamanlaması (Yeni Mantık)")]
    public float saldiriSuresi = 0.8f;    // Saldırı animasyonunun gerçek süresi
    public float saldiriBeklemeSuresi = 0.5f; // İki saldırı arası zorunlu bekleme (Cooldown)
    private float saldiriZamanlayici;
    private bool saldiriyorMu = false;

    [Header("Zaman Ayarlı Takip Mantığı")]
    public float kovalamaBitisSuresi = 8f; 
    private float zemindenUzaktaGecenSure = 0f;

    [Header("Referanslar")]
    public Transform hedefOyuncu;     

    [Header("Saldırı Etkileri")]
    public float vurusFirlatmaGucu = 15f; // Inspector'dan bu değeri kendine göre ayarlayabilirsin         

    private NavMeshAgent agent;
    private CatStateController durumKontrolcu;
    private float devriyeZamanlayici;
    
    public bool kovaliyorMu { get; private set; } = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        durumKontrolcu = GetComponent<CatStateController>();

        if (hedefOyuncu == null)
            hedefOyuncu = GameObject.FindGameObjectWithTag("Player")?.transform;

        agent.speed = devriyeHizi;
        agent.acceleration = ivmelenme;
        agent.angularSpeed = donusHizi;
        agent.stoppingDistance = saldiriMesafesi - 0.2f;

        devriyeZamanlayici = gezmeAraligi;
    }

    void Update()
    {
        // OYUN DURDUYSA KEDİNİN DÜŞÜNMESİNİ ENGELLE
        if (GameManager.Instance.GetCurrentGameState() != GameState.Playing &&
            GameManager.Instance.GetCurrentGameState() != GameState.Resume)
        {
            return; 
        }

        HandleAttackTimers(); 

        if (saldiriyorMu) return; // Saldırı yaparken hareket etme veya düşünme

        if (kovaliyorMu)
        {
            zemindenUzaktaGecenSure += Time.deltaTime;
            if (zemindenUzaktaGecenSure >= kovalamaBitisSuresi)
            {
                KovalamayiDurdur();
            }
            else
            {
                KovalamaMantigi();
            }
        }
        else
        {
            DevriyeMantigi();
        }
    }

    private void HandleAttackTimers()
    {
        if (saldiriyorMu)
        {
            saldiriZamanlayici -= Time.deltaTime;
            if (saldiriZamanlayici <= 0)
            {
                saldiriyorMu = false;
                saldiriZamanlayici = saldiriBeklemeSuresi;
                
                // Araya idle girmemesi için saldırı biter bitmez direkt koşmaya devam et
                if (kovaliyorMu)
                {
                    agent.isStopped = false;
                    agent.speed = kovalamaHizi;
                    durumKontrolcu.ChangeState(CatState.Running);
                }
            }
        }
        else if (saldiriZamanlayici > 0)
        {
            saldiriZamanlayici -= Time.deltaTime;
        }
    }

    // FloorTrigger'ın çağırdığı eksik fonksiyon
    public void ZemineDokunuldu()
    {
        zemindenUzaktaGecenSure = 0f; 
        if (!kovaliyorMu) KovalamayaBasla();
    }

    private void DevriyeMantigi()
{
    devriyeZamanlayici += Time.deltaTime;

    // --- TAKILMA KONTROLÜ BAŞLANGIÇ ---
    // Eğer kedi bir yere gitmeye çalışıyorsa ama hızı çok düşükse (bir yere takıldıysa)
    if (agent.hasPath && agent.velocity.magnitude < 0.2f)
    {
        takilmaZamanlayici += Time.deltaTime;
        
        // Belirlenen süreden fazla takılı kaldıysa yeni bir nokta seçmeye zorla
        if (takilmaZamanlayici >= maxTakilmaSuresi)
        {
            agent.ResetPath(); // Mevcut yolu iptal et
            devriyeZamanlayici = gezmeAraligi; // Zamanlayıcıyı doldur ki aşağıdaki if çalışsın
            takilmaZamanlayici = 0f;
        }
    }
    else
    {
        takilmaZamanlayici = 0f; // Hareket ediyorsa zamanlayıcıyı sıfırla
    }
    // --- TAKILMA KONTROLÜ BİTİŞ ---

    if (devriyeZamanlayici >= gezmeAraligi && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
    {
        Vector3 yeniNokta = RastgeleNoktaBul(transform.position, gezmeYaricapi);
        agent.SetDestination(yeniNokta);
        devriyeZamanlayici = 0f;
    }

    CatState yeniDurum = agent.velocity.magnitude > 0.1f ? CatState.Walking : CatState.Idle;
    durumKontrolcu.ChangeState(yeniDurum);
}

    private void KovalamaMantigi()
    {
        if (hedefOyuncu == null) return;

        agent.SetDestination(hedefOyuncu.position);
        float mesafe = Vector3.Distance(transform.position, hedefOyuncu.position);
        
        if (mesafe <= saldiriMesafesi)
        {
            if (saldiriZamanlayici <= 0)
            {
                // ESKİ SATIRI SİLİYORUZ: BakmaYonuGuncelle(hedefOyuncu.position);
                
                agent.isStopped = true; 
                agent.ResetPath(); 
                
                durumKontrolcu.ChangeState(CatState.Attacking);
                saldiriyorMu = true;
                saldiriZamanlayici = saldiriSuresi; 

                // --- 1. DOTWEEN İLE YAYLANARAK HEDEFE KİLİTLENME ---
                // Y eksenini sabit tutuyoruz ki kedi yere/havaya doğru eğilmesin
                Vector3 lookPos = new Vector3(hedefOyuncu.position.x, transform.position.y, hedefOyuncu.position.z);
                transform.DOLookAt(lookPos, 0.2f).SetEase(Ease.OutBack);

                // --- 2. DOTWEEN İLE İLERİ ATILMA (LUNGE) ---
                // Oyuncunun olduğu yöne doğru 0.7 birimlik sert bir atılma ve geri çekilme
                Vector3 lungeDir = (lookPos - transform.position).normalized * 0.7f;
                transform.DOPunchPosition(lungeDir, 0.3f, 1, 0.5f);

                // --- 3. DOTWEEN İLE ESNEME (SQUASH & STRETCH) ---
                // Kedi saldırı anında hafifçe uzayıp basıklaşır, vuruşa ağırlık katar
                transform.DOPunchScale(new Vector3(0.2f, -0.2f, 0.2f), 0.3f, 1, 0.5f);


                // --- HASAR VE FIRLATMA İŞLEMLERİ ---
                HealthManager.Instance.Damage(1); 
                
                Rigidbody playerRb = hedefOyuncu.GetComponent<Rigidbody>();
                if (playerRb != null)
                {
                    Vector3 firlatmaYonu = (hedefOyuncu.position - transform.position).normalized;
                    firlatmaYonu.y = 0.5f; 
                    
                    playerRb.linearVelocity = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z);
                    playerRb.AddForce(firlatmaYonu.normalized * vurusFirlatmaGucu, ForceMode.Impulse);
                }
            }
            else
            {
                // Saldırı bekleme süresindeyse (cooldown), oyuncuyu takip ederken normal dönmeye devam etsin
                BakmaYonuGuncelle(hedefOyuncu.position);
            }
        }
        else
        {
            if (saldiriZamanlayici <= 0)
            {
                agent.isStopped = false;
                agent.speed = kovalamaHizi;
                durumKontrolcu.ChangeState(CatState.Running);
            }
        }
    }

    private void BakmaYonuGuncelle(Vector3 hedef)
    {
        Vector3 yon = (hedef - transform.position).normalized;
        yon.y = 0;
        if (yon != Vector3.zero)
        {
            Quaternion bakisAcisi = Quaternion.LookRotation(yon);
            transform.rotation = Quaternion.Slerp(transform.rotation, bakisAcisi, Time.deltaTime * 15f);
        }
    }

    public void KovalamayaBasla()
    {
        kovaliyorMu = true;
        agent.isStopped = false;
        agent.speed = kovalamaHizi;
    }

    public void KovalamayiDurdur()
    {
        kovaliyorMu = false;
        
        if (agent.isActiveAndEnabled)
        {
            agent.isStopped = false;
            agent.ResetPath();
            agent.speed = devriyeHizi;
        }

        saldiriyorMu = false; 
        saldiriZamanlayici = 0;
        durumKontrolcu.ChangeState(CatState.Idle);

        devriyeZamanlayici = gezmeAraligi; 
        zemindenUzaktaGecenSure = 0f;
    }

    private Vector3 RastgeleNoktaBul(Vector3 merkez, float yaricap)
    {
        Vector3 rastgeleYon = Random.insideUnitSphere * yaricap;
        rastgeleYon += merkez;
        if (NavMesh.SamplePosition(rastgeleYon, out NavMeshHit hit, yaricap, 1)) return hit.position;
        return merkez;
    }

    private void OnDrawGizmos()
{
    // Agent henüz oluşmadıysa veya sahnede değilse hata vermemesi için kontrol
    if (agent == null || !agent.isOnNavMesh) return;

    // 1. HEDEF NOKTAYI ÇİZ (Küre şeklinde)
    // Eğer kovalıyorsa kırmızı, devriyedeyse cam göbeği renginde görünsün
    Gizmos.color = kovaliyorMu ? Color.red : Color.cyan;
    Gizmos.DrawSphere(agent.destination, 0.4f);

    // 2. MEVCUT ROTAYI ÇİZ (Çizgi şeklinde)
    // NavMesh'in hesapladığı tüm köşe noktalarını birleştirir
    if (agent.hasPath)
    {
        Gizmos.color = kovaliyorMu ? Color.red : Color.yellow;
        Vector3[] corners = agent.path.corners;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
        }
    }

    // 3. DEVRIYE ALANINI ÇİZ (Tel küre şeklinde)
    // Kedinin o anki konumuna göre ne kadarlık bir alanda rastgele nokta aradığını gösterir
    Gizmos.color = new Color(0, 1, 0, 0.3f); // Şeffaf yeşil
    Gizmos.DrawWireSphere(transform.position, gezmeYaricapi);
}
}