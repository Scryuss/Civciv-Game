using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator _playerAnimator;

    PlayerController _playerController;
    StateController _stateController;

    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stateController = GetComponent<StateController>();
    }

    void Start()
    {
        _playerController.OnPlayerJumped += PlayerController_OnPlayerJumped;
    }

    void Update()
    {
        SetPlayerAnimation();
    }

    void PlayerController_OnPlayerJumped()
    {
        _playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, true);
        Invoke(nameof(ResetJumpingAnimation), 0.5f);
    }

    private void ResetJumpingAnimation()
    {
        _playerAnimator.SetBool(Consts.PlayerAnimations.IS_JUMPING, false);
    }

    void SetPlayerAnimation()
    {
        var currentState = _stateController.GetCurrentPlayerState();

        switch (currentState)
        {
            case PlayerState.Idle:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, false);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                break;

            case PlayerState.Move:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_MOVING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, false);
                break;
                
            case PlayerState.SlideIdle:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, false);
                break;

            case PlayerState.Slide:
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING, true);
                _playerAnimator.SetBool(Consts.PlayerAnimations.IS_SLIDING_ACTIVE, true);
                break;
        }
    }
}
