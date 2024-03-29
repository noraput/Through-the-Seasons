using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons {
    public class Jumping : CharacterAbility
    {   
        [Header("General")]
        [SerializeField]
        protected float jumpForce = 500f;

        [SerializeField]
        protected float holdJumpForce = 0.01f; 

        [SerializeField]
        protected float defaultHoldTime = 0.2f;
        private float flyingHoldTime = 999f;

        [SerializeField]
        protected int maxJumps = 2;
        private int flyingMaxJumps = 999;

        [SerializeField]
        protected float defaultJumpBufferTime = 0.2f;

        [SerializeField]
        protected float fallSpeedLimit = 30f;

        [SerializeField]
        protected float thrustForce = 10f;

        [SerializeField]
        private float thrustMaxHoldTime = 0.5f;

        [SerializeField]
        private float thrustMaxReleaseTime = 0.5f;
        
        [SerializeField]
        protected float glideSpeedMultiplier;

        [SerializeField]
        protected bool allowsHoldJump = true;

        [SerializeField]
        protected bool allowsMultiJump = true;

        [Header("Ground Check")]
        [SerializeField]
        protected Transform groundCheck;

        public float groundCheckRadius = 0.3f;

        [SerializeField]
        protected LayerMask groundLayer;

        public bool isJumping;
        public bool pressedJumpKey;

        public float currentHoldTime;
        public float jumpMultiplier = 1f;
        public int jumpsLeft;
        public float jumpBufferTime;
        
        public float coyoteTime;
        private bool isCoyote;
        private float fallingTime;
        private bool isMidair;
        private bool isFalling;
        private bool hasFallen;

        private float thrustHoldTime;
        private float thrustReleaseTime;
        
        protected override void Initialize() {
            base.Initialize();
        }

        protected virtual void Update() {
            if (PlayerCore.instance.CompareState(PlayerState.Flying)) {
                if (Input.GetKey(KeyCode.Space)) {
                    rb.velocity = new Vector2(
                        rb.velocity.x,
                        thrustForce
                    );
                }
                else {
                    rb.velocity = new Vector2(
                        rb.velocity.x,
                        -thrustForce
                    );
                }
            }

            // Test();
            InputJump();
        }

        protected virtual void LateUpdate() {
            SetAnimation();
            // Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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
            HandleCoyoteTime();
            HandleJumpBuffer();
        }

        protected virtual void Test() {
            if (Input.GetKeyDown(KeyCode.M)) {
                anim.SetTrigger("Hit");
            }
        }

        protected virtual void SetAnimation() {
            anim.SetFloat("Y Speed", rb.velocity.y);
            anim.SetBool("Is Grounded", character.IsGrounded);
        }

        protected virtual void InputJump() {
            if (!PlayerCore.instance.CompareState(PlayerState.Running))
                return;

            if (Input.GetKeyDown(KeyCode.Z)) {
                if (jumpsLeft <= 0) {
                    if (!pressedJumpKey) {
                        pressedJumpKey = true;
                        jumpBufferTime = defaultJumpBufferTime;
                    }

                    isJumping = false;
                    return;
                }

                if (jumpsLeft >= maxJumps) {
                    GameStats.instance.AddAchievementProgress("DoubleJumpsPerformed");
                }

                currentHoldTime = 0;
                isJumping = true;

                --jumpsLeft;
                anim.SetTrigger("Jump");
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
            if (jumpBufferTime > 0) {
                jumpBufferTime -= Time.fixedDeltaTime;
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

                anim.SetTrigger("Jump Buffer");
            }
        }

        protected virtual void HandleCoyoteTime() {
            if (isJumping && character.IsGrounded) {
                isMidair = true;
            }

            if (!character.IsGrounded & !isMidair) {
                if (fallingTime > coyoteTime) {
                    if (!hasFallen)
                        Fall();

                    return;
                }

                fallingTime += Time.fixedDeltaTime;
                isCoyote = true;
            }
        }

        protected virtual void Fall() {
            isFalling = true;
            isCoyote = false;
            jumpsLeft = hasFallen ? jumpsLeft : 1;
            hasFallen = true;
        }

        protected virtual void GroundCheck() {
            if (IsTouchingGround() && !isJumping) {
                character.IsGrounded = true;
                // pressedJumpKey = false;
                isMidair = false;
                isFalling = false;
                hasFallen = false;

                jumpsLeft = maxJumps; 
                fallingTime = 0f;
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
