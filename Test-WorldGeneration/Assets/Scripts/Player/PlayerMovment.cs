using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInteractHandeler))]
public class PlayerMovment : MonoBehaviour
{
    [Header("Move settings")]
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float MaxStepLenth = 10f;
    private CharacterController Controller;
    private PlayerInteractHandeler playerInteractHandeler;
    private Vector3 Direction;
    private Vector3 PlayerVelocity;

    private void Awake()
    {
        Controller = GetComponent<CharacterController>();
        playerInteractHandeler = GetComponent<PlayerInteractHandeler>();
    }
    void FixedUpdate()
    {
        Vector3 targetVelocity = Quaternion.Euler(0, 45, 0) * new Vector3(Direction.x, 0, Direction.y);

        PlayerVelocity = Vector3.MoveTowards(PlayerVelocity, targetVelocity, MaxStepLenth * Time.deltaTime);

        Controller.SimpleMove(PlayerVelocity * MoveSpeed);

        RotatePlayer();
    }
    private void RotatePlayer()
    {
        Quaternion targetRotation;

        if (playerInteractHandeler.IsGrabing && playerInteractHandeler.HasInteractable)
        {
            Vector3 LookTowardsDirection = playerInteractHandeler.WorldGrabPoint - transform.position;
            LookTowardsDirection.y = 0;

            targetRotation = Quaternion.LookRotation(LookTowardsDirection);
        }
        else
        {
            if (PlayerVelocity.sqrMagnitude < 0.001f) return;

            targetRotation = Quaternion.LookRotation(PlayerVelocity);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }

    public void MovePlayer(InputAction.CallbackContext callbackContext)
    {
        Direction = callbackContext.ReadValue<Vector2>();
    }
}