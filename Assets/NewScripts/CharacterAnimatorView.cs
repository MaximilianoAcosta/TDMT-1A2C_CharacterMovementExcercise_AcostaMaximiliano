using System;
using System.Collections;
using UnityEngine;

namespace Movement
{
    public class CharacterAnimatorView : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Animator animator;

        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private PlayerJumpController playerJumpController;
        [SerializeField] private PlayerBody body;

        [Header("Animator Parameters")]
        [SerializeField] private string jumpTriggerParameter ;
        [SerializeField] private string isFallingParameter;
        [SerializeField] private string horSpeedParameter;
        [SerializeField] private bool fallTrigger;
        private void OnEnable()
        {
            playerJumpController =transform.parent.GetComponent<PlayerJumpController>();
            if (playerJumpController)
            {
                
                playerJumpController.OnJump += HandleJump;
                playerJumpController.OnLand += CheckLand;
            }
        }

        private void OnDisable()
        {
            if (playerJumpController)
            {
                playerJumpController.OnJump -= HandleJump;
            }
        }

        private void Update()
        {
            if (!rigidBody)
                return;
            Vector3 velocity = rigidBody.velocity;
            velocity.y = 0;
            float speed = velocity.magnitude;
            if (animator)
                animator.SetFloat(horSpeedParameter, speed);
            if (animator && body)
            {
                animator.SetBool(isFallingParameter, body.isFalling);
            }
            if (body.isFalling && rigidBody.velocity.y < 0 && fallTrigger)
            {
                fallTrigger = false;
                animator.SetTrigger(jumpTriggerParameter);
            }
            
        }

        private void HandleJump()
        {
            if (animator)
            {
                fallTrigger = false;
                animator.SetTrigger(jumpTriggerParameter);
            }
        }
        private void CheckLand()
        {
            fallTrigger = true;
        }
    }
}