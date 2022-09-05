using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ThroughTheSeasons {
    public class HorizontalMovement : CharacterAbility
    {
        [SerializeField]
        protected float runSpeed = 500f;
        public float RunSpeed { get; set; }

        [SerializeField]
        protected float sprintMultiplier = 1.5f;

        [SerializeField]
        protected float movementSmoothing = 0.05f;

        [SerializeField]
        protected int coyotyFrames = 5;

        public float horizontalInput;
        public Vector3 velocity;

        public int framesLeft;
        public bool isCoyoty;

        public float speedMultiplier = 1f;
        private bool isStop = false;

        [SerializeField]
        private float footstepDefaultDuration = 0.25f;
        private float footstepTime;
        
        protected virtual void Update() {
            isCoyoty = MovementPressedWithinFrames();

            InputMovement();
            CheckDirection();
        }

        protected virtual void InputMovement() {
            horizontalInput = !isStop ? Input.GetAxisRaw("Horizontal") : 0f;

            // if (horizontalInput != 0f) {
            //     CheckForFootstep();
            // }
            // else {
            //     footstepTime = footstepDefaultDuration;
            // }

            if (anim) {
                anim?.SetFloat("Horizontal Speed", Mathf.Abs(horizontalInput));
                return;
            }
        }

        // protected virtual void CheckForFootstep() {
        //     if (character.IsGrounded) {
        //         footstepTime -= Time.deltaTime;
        //     }
            
        //     if (footstepTime <= 0) {
        //         AudioManager.instance.PlayFootstep();
        //         footstepTime = footstepDefaultDuration;
        //     }
        // }

        public virtual bool MovementPressedWithinFrames() {
            if (horizontalInput != 0) {
                framesLeft = coyotyFrames;
                return true;
            }
            else if (framesLeft > 0) {
                --framesLeft;
                return true;
            }
            
            return false;
        }

        public virtual bool IsInCoyotyTime() {
            return isCoyoty;
        }

        public virtual bool RightMovementPressed() {
            return horizontalInput > 0;
        }

        public virtual bool LeftMovementPressed() {
            return horizontalInput < 0;
        }

        protected virtual void FixedUpdate() {
            Move();
        }

        protected virtual void Move() {
            // if (character.main.CompareState(CharacterState.Knockback))
            //     return;

            // CheckForSprint();
            
            Vector2 targetVelocity = new Vector2(horizontalInput * runSpeed * Time.fixedDeltaTime * speedMultiplier, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        // protected virtual void CheckForSprint() {
        //     if (Input.GetKey(KeyCode.LeftShift) && character.IsGrounded) {
        //         horizontalInput *= sprintMultiplier;
        //     }
        // }

        protected virtual void CheckDirection() {
            if (MoveOppositeToFacingDirection()) {
                character.Flip();
            }
        }

        public virtual void Stop() {
            isStop = true;
            horizontalInput = 0f;
            rb.velocity = new Vector2 (0, rb.velocity.y);

            if (anim) {
                anim?.SetFloat("Horizontal Speed", 0f);
                return;
            }
            
            enabled = false;
        }

        public virtual void ResetStopping() {
            isStop = false;
            enabled = true;
        } 

        // public virtual void SetInputMultiplier(float multiplier) {
        //     speedMultiplier = multiplier * GameManager.instance.skillTreeSystem.passiveSkillManager.GetSpeedMultiplier();
        // }

        // public virtual void UpdateMovementSpeed() {
        //     speedMultiplier *= GameManager.instance.skillTreeSystem.passiveSkillManager.GetSpeedMultiplier();
        // }

        private bool MoveOppositeToFacingDirection() {
            return (horizontalInput > 0f && !character.IsFacingRight) || (horizontalInput < 0f && character.IsFacingRight);
        }
    }
}