using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Godchild {
    public class Jumping : CharacterAbility
    {   
        [Header("General")]
        [SerializeField]
        protected float jumpForce = 500f;

        [SerializeField]
        protected float holdJumpForce = 0.01f;

        [SerializeField]
        protected float defaultHoldTime = 0.2f;

        [SerializeField]
        protected int maxJumps = 2;

        [SerializeField]
        protected int defaultJumpBufferFrames = 10;

        [SerializeField]
        protected float fallSpeedLimit = 30f;
        
        [SerializeField]
        protected float glideSpeedMultiplier;

        [SerializeField]
        protected bool allowsHoldJump = true;

        [SerializeField]
        protected bool allowsMultiJump = true;

        [Header("Ground Check")]
        [SerializeField]
        protected Transform groundCheck;

        [SerializeField]
        protected float groundCheckRadius = 0.3f;

        [SerializeField]
        protected LayerMask groundLayer;

        public bool isJumping;
        public bool pressedJumpKey;

        public float currentHoldTime;
        public float jumpMultiplier = 1f;
        public int jumpsLeft;
        public int jumpBufferFrames;
        
        public int coyotyFrames;
        private bool isCoyoty;
        private int fallingFrames;
        private bool isMidair;
        private bool isFalling;
        private bool hasFallen;
        
        protected override void Initialize() {
            base.Initialize();
        }

        protected virtual void Update() {
            InputJump();
            HandleCoyotyTime();
            HandleJumpBuffer();
        }

        public virtual void Stop() {
            isJumping = false;
            enabled = false;
        }

        public virtual void ResetStopping() {
            enabled = true;
        }
        
        protected virtual void FixedUpdate() {
            Jump();
            GroundCheck();
            LimitFallSpeed();
        }

        protected virtual void InputJump() {
            if (Input.GetKeyDown(KeyCode.Z)) {
                if (jumpsLeft <= 0) {
                    if (!pressedJumpKey) {
                        pressedJumpKey = true;
                        jumpBufferFrames = defaultJumpBufferFrames;
                    }

                    isJumping = false;
                    return;
                }

                currentHoldTime = 0;
                isJumping = true;

                --jumpsLeft;
                
                //PlaySound();
            }
        }

        // protected virtual void PlaySound() {
        //     AudioManager.instance.RandomPlayOneShot("Jump");
        // }

        protected virtual bool InputHoldJump() {
            if (Input.GetKey(KeyCode.Z)) {
                return true;
            }

            return false;
        }

        protected virtual void Jump() {
            if (isJumping) {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce * jumpMultiplier);

                HandleHoldJump();  
            }
        }

        protected virtual void HandleHoldJump() {
            if (!allowsHoldJump || holdJumpForce <= 0f) {
                isJumping = false;
                return;
            }

            if (!InputHoldJump()) {
                isJumping = false;
                return;
            }

            currentHoldTime += Time.deltaTime;

            if (currentHoldTime >= defaultHoldTime) {
                currentHoldTime = 0;
                isJumping = false;

                return;
            }
            
            rb.AddForce(Vector2.up * holdJumpForce);
        }

        protected virtual void HandleJumpBuffer() {
            if (jumpBufferFrames > 0) {
                jumpBufferFrames--;
            }
            else {
                pressedJumpKey = false;
            }
            
            if (IsTouchingGround() && pressedJumpKey) {
                isJumping = true;
                pressedJumpKey = false;
                currentHoldTime = 0;

                jumpsLeft = maxJumps;
                jumpsLeft--;
            }
        }

        protected virtual void HandleCoyotyTime() {
            if (isJumping && character.IsGrounded) {
                isMidair = true;
            }

            if (!character.IsGrounded & !isMidair) {
                if (fallingFrames > coyotyFrames) {
                    if (!hasFallen)
                        Fall();

                    return;
                }

                fallingFrames++;
                isCoyoty = true;
            }
        }

        protected virtual void Fall() {
            isFalling = true;
            isCoyoty = false;
            jumpsLeft = hasFallen ? jumpsLeft : 1;
            hasFallen = true;
        }

        protected virtual void GroundCheck() {
            if (IsTouchingGround() && !isJumping) {
                character.IsGrounded = true;
                pressedJumpKey = false;
                isMidair = false;
                isFalling = false;
                hasFallen = false;

                jumpsLeft = maxJumps; 
                fallingFrames = 0;
            }
            else {
                character.IsGrounded = false;
            }
        }

        protected virtual bool IsTouchingGround() {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        protected virtual void LimitFallSpeed() {
            if (character.IsGrounded)
                return;

            if (rb.velocity.y < -fallSpeedLimit) {
                rb.velocity = new Vector2(rb.velocity.x, -fallSpeedLimit);
            }	
        }

        public virtual void SetJumpMultiplier(float multiplier) {
            jumpMultiplier = multiplier;
        }

        public virtual void SetJumping(bool isJumping) {
            this.isJumping = true;
        }

        #region Debug
        protected void OnDrawGizmos() {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        #endregion
    }
}
