using UnityEngine;

public class CatStateController : MonoBehaviour
{
    [Header("Mevcut Durum")]
    [SerializeField] private CatState _currentState = CatState.Idle;
    public CatState CurrentState => _currentState;

    private Animator animator;

    // Performans için String yerine Hash kullanıyoruz
    private static readonly int IdlingHash = Animator.StringToHash("IsIdling");
    private static readonly int WalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int RunningHash = Animator.StringToHash("IsRunning");
    private static readonly int AttackingHash = Animator.StringToHash("IsAttacking");

    void Awake()
    {
        // Kedi hiyerarşisinde Animator nerede olursa olsun bulmaya çalış (CatVisual gibi)
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError($"{name} üzerinde Animator bulunamadı! Lütfen CatVisual objesinde Animator olduğundan emin ol.");
    }

    // ====================== STATE DEĞİŞTİRME ======================
    public void ChangeState(CatState newState)
    {
        if (_currentState == newState) return;

        _currentState = newState;
        UpdateAnimatorParameters();
    }

    // ====================== ANIMATOR ======================
    private void UpdateAnimatorParameters()
    {
        if (animator == null) return;

        // Sadece durum değiştiğinde tüm parametreleri tek bir karede günceller
        animator.SetBool(IdlingHash,    _currentState == CatState.Idle);
        animator.SetBool(WalkingHash,   _currentState == CatState.Walking);
        animator.SetBool(RunningHash,   _currentState == CatState.Running);
        animator.SetBool(AttackingHash, _currentState == CatState.Attacking);
    }
}