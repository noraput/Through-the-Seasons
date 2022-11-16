using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Dodging : CharacterAbility
    {
        [SerializeField]
        protected float gravityMultiplier;

        [SerializeField]
        protected float dodgingColliderHeight;

        protected float dodgingGravity;
        protected float defaultGravity;

        protected Vector2 defaultColliderSize;

        protected override void Initialize() {
            base.Initialize();
            
            defaultGravity = rb.gravityScale;
            dodgingGravity = defaultGravity * gravityMultiplier;
            defaultColliderSize = collider2d.size;
        }

        protected void Update() {
            InputDodge();
        }

        protected void InputDodge() {
            if (!PlayerCore.instance.CompareState(PlayerState.Running))
                return;

            if (!Input.GetKey(KeyCode.X)) {
                rb.gravityScale = defaultGravity;

                collider2d.size = defaultColliderSize;
                character.IsDodging = false;
                anim.SetBool("Is Dashing", false);
                return;
            }

            if (!character.IsGrounded) {
                rb.gravityScale = dodgingGravity;
                character.IsDodging = false;
                anim.SetBool("Is Dashing", false);
                return;
            }

            collider2d.size = new Vector2(collider2d.size.x, dodgingColliderHeight);
            character.IsDodging = true;

            anim.SetBool("Is Dashing", true);
        }
    }
}
