using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ThroughTheSeasons {
    public class HorizontalMovement : CharacterAbility
    {
        [SerializeField]
        protected float speedIncreaseRate = 0.1f;
        protected float currentSpeed;

        [SerializeField]
        protected float defaultRunSpeed = 10f;
        public float DefaultRunSpeed { get => defaultRunSpeed; }
        
        public float horizontalInput;
        public Vector3 velocity;

        public float speedMultiplier = 1f;
        private float runTime;
        private float defaultGravity;

        [SerializeField]
        float PrepareFlyingStopHeight;

        // [SerializeField]
        // private float footstepDefaultDuration = 0.25f;
        // private float footstepTime;

        public virtual void Reset() {
            runTime = 0f;
            currentSpeed = defaultRunSpeed;
            defaultGravity = rb.gravityScale;
        }
        
        protected override void Initialize() {
            base.Initialize();
            Reset();
        }

        protected virtual void FixedUpdate() {
            Move();
        }

        protected virtual void Move() {
            if (PlayerCore.instance.CompareState(PlayerState.PrepareFlying)) {
                rb.gravityScale = 0f;
                rb.velocity = new Vector2(currentSpeed, 5f);

                if (transform.position.y >= PrepareFlyingStopHeight) {
                    PlayerCore.instance.ChangeState(PlayerState.Flying);
                    return;
                }

                return;
            }

            runTime += Time.fixedDeltaTime;
            currentSpeed = (defaultRunSpeed + (speedIncreaseRate * Mathf.Sqrt(runTime))) * speedMultiplier;
            Vector2 targetVelocity = new Vector2(currentSpeed, rb.velocity.y);
            
            //Debug.Log(currentSpeed);
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
            
            rb.velocity = targetVelocity;
        }
    }
}