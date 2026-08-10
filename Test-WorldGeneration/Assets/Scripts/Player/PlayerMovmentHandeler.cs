using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float MaxStepLenth = 10f;
    private CharacterController Controller;
    private Vector3 Direction;
    private Vector3 PlayerPosition;

    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
        Vector3 targetVelocity = Quaternion.Euler(0, 45, 0) * new Vector3(Direction.x, 0, Direction.y);

        PlayerPosition = Vector3.MoveTowards(PlayerPosition, targetVelocity, MaxStepLenth * Time.deltaTime);

        Controller.SimpleMove(PlayerPosition * MoveSpeed);

        if (PlayerPosition.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(PlayerPosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
    public void MovePlayer(InputAction.CallbackContext callbackContext)
    {
        Direction = callbackContext.ReadValue<Vector2>();
    }
}