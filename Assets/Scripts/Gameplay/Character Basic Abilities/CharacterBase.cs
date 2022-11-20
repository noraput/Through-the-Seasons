using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughTheSeasons {
    public class CharacterBase : MonoBehaviour
    {
        private bool isFacingRight;
        public bool IsFacingRight {
            get => isFacingRight; 
            set => isFacingRight = value;
        }

        private bool isGrounded;
        public bool IsGrounded {
            get => isGrounded;
            set => isGrounded = value;
        }

        private bool isDodging;
        public bool IsDodging {
            get => isDodging;
            set => isDodging = value;
        }

        protected BoxCollider2D collider2d;

        public Rigidbody2D rb;

        protected HorizontalMovement movement;
        public HorizontalMovement Movement { get => movement; } 

        protected Jumping jumping;
        public Jumping Jumping { get => jumping; }

        protected Dodging dodging;
        public Dodging Dodging { get => dodging; }

        // [HideInInspector]
        // public Character main;

        protected Animator anim;
        public Animator Anim { get => anim; }

        private bool isInitialized;

        protected virtual void Start() {
            if (!isInitialized) {
                Initialize();
                isInitialized = true;
            }
        }

        protected virtual void OnEnable() {
            if (!isInitialized) {
                Initialize();
                isInitialized = true;
            }
        }

        protected virtual void Initialize() {            
            rb = transform.GetComponent<Rigidbody2D>();
            collider2d = transform.GetComponent<BoxCollider2D>();

            movement = GetComponent<HorizontalMovement>();
            jumping = GetComponent<Jumping>();
            dodging = GetComponent<Dodging>();

            anim = transform.Find("Anim").GetComponent<Animator>();
            Eneble();
        }

        public virtual void Disable() {
            movement.Stop();

            movement.enabled = false;
            jumping.enabled = false;
            dodging.enabled = false;
            anim.enabled = false;
        }

        public virtual void Eneble() {
            movement.Reset();

            movement.enabled = true;
            jumping.enabled = true;
            dodging.enabled = true;
            anim.enabled = true;
        }

        public virtual void Flip() {
            // if (main.CompareState(CharacterState.Knockback))
            //     return;

            isFacingRight = !isFacingRight;
            transform.Rotate(0, 180, 0);
        }

        protected virtual void SetFallSpeedMultiplier(float fallSpeedMultiplier) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallSpeedMultiplier);
        }

        protected virtual bool CollisionCheck(Vector2 direction, float distance, LayerMask collisionLayer) {
            RaycastHit2D[] hits = new RaycastHit2D[10];
            int numberOfHits = collider2d.Cast(direction, hits, distance);

            for (int i = 0; i < numberOfHits; ++i) {
                if ((1 << hits[i].collider.gameObject.layer & collisionLayer) != 0) {
                    return true;
                }
            }
            return false;
        }
    }
}
