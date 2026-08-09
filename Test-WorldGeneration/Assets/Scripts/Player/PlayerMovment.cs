using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovment : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 5f;
    private CharacterController Controller;
    private Vector3 Direction;
    private Vector3 PlayerPosition;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float acceleration = 15f;
    public enum InputType { Keyboard, Controller }
    public InputType SelectedInputType = InputType.Controller;
    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        Direction = CalculateMovement(SelectedInputType.ToString());

        Vector3 targetVelocity = Quaternion.Euler(0, 45, 0) * Direction;

        float rate = targetVelocity.magnitude > 0 ? acceleration : deceleration;

        PlayerPosition = Vector3.MoveTowards(PlayerPosition, targetVelocity, rate * Time.deltaTime);

        Controller.SimpleMove(PlayerPosition * MoveSpeed);
    }
    private Vector3 CalculateMovement(string InputType)
    {
        Vector3 input = Vector3.zero;

        // Keyboard input
        if (Keyboard.current != null && InputType == "Keyboard")
        {
            if (Keyboard.current.wKey.isPressed) input.z += 1;
            if (Keyboard.current.sKey.isPressed) input.z -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        // Controler input
        if (Gamepad.current != null && InputType == "Controller")
        {
            Vector2 leftStickInput = Gamepad.current.leftStick.ReadValue();

            input.x = leftStickInput.x;
            input.z = leftStickInput.y;
        }

        return input;
    }
}