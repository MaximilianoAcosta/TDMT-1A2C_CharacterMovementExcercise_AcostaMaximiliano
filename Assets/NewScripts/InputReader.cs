using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inputs
{
    public class InputReader : MonoBehaviour
    {
        public event Action<Vector2> OnMovementInput = delegate { };
        public event Action<Vector2 > OnCameraRotation = delegate { };
        public event Action OnJumpInput = delegate { };
        public event Action OnSprintStart = delegate { };
        public event Action OnSprintEnd = delegate { };
        private Vector2 lastMovement;
        public void HandleMovementInput(InputAction.CallbackContext ctx)
        {
            lastMovement = ctx.ReadValue<Vector2>();
            OnMovementInput?.Invoke(lastMovement);
        }
        public void HandleJumpInput(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                OnJumpInput?.Invoke();
            }
        }
        public void HandleCameraRotation(InputAction.CallbackContext ctx)
        {
            OnCameraRotation?.Invoke(ctx.ReadValue<Vector2>());
            OnMovementInput?.Invoke(lastMovement);
        }
        public void HandleSprint(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                Debug.Log("Sprint called");
                OnSprintStart?.Invoke();
                OnMovementInput?.Invoke(lastMovement);
            }

            if (ctx.canceled)
            {
                Debug.Log("Sprint stopped");
                OnSprintEnd?.Invoke();
                OnMovementInput?.Invoke(lastMovement);
            }
        }
    }
}
