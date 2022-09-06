using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons {
    public class CharacterAbility : CharacterBase
    {
        protected CharacterBase character;
        
        protected override void Initialize() {
            base.Initialize();
            character = GetComponent<CharacterBase>();

            // Debug.Log(this + " is Initialized");
        }
    }
}
