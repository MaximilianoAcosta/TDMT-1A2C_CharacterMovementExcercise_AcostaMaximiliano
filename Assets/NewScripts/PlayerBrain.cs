using Inputs;
using Movement;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [SerializeField] private PlayerBody body;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerJumpController jumpBehaviour;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float normalSpeed = 10;
    [SerializeField] private float sprintSpeed = 15;
    [SerializeField] private float NormalAcceleration;
    [SerializeField] private float SprintAcceleration = 1;

    private float speed = 10;
    private float acceleration = 20;
    private Vector3 desiredDirection;



    private void OnEnable()
    {
        if (body == null|| !inputReader)
        {
            enabled = false;
            return;
        }

        inputReader.OnMovementInput += HandleMovementInput;
        inputReader.OnJumpInput += HandleJumpInput;
        inputReader.OnSprintStart += StartSprint;
        inputReader.OnSprintEnd += EndSprint;
    }
    private void OnDisable()
    {
        if (!inputReader)
        {
            return;
        }
        inputReader.OnMovementInput -= HandleMovementInput;
        inputReader.OnJumpInput -= HandleJumpInput;
        inputReader.OnSprintStart -= StartSprint;
        inputReader.OnSprintEnd -= EndSprint;
    }


    private void HandleMovementInput(Vector2 input)
    {

        if (desiredDirection.magnitude > Mathf.Epsilon && input.magnitude < Mathf.Epsilon)
        {
            body.RequestBrake();
        }

        desiredDirection = new Vector3(input.x, 0, input.y);

        if (cameraTransform)
        {
            desiredDirection = cameraTransform.TransformDirection(desiredDirection);
            desiredDirection.y = 0;
        }
        body.SetMovement(new MovementRequest(desiredDirection, speed, acceleration));
    }
    private void HandleJumpInput()
    {
        jumpBehaviour.TryJump();
    }
    private void StartSprint()
    {
        speed = sprintSpeed;
        acceleration = SprintAcceleration;
    }
    private void EndSprint()
    {
        speed = normalSpeed;
        acceleration = NormalAcceleration;
    }
}
