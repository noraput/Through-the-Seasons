using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Rocket : TemporaryItem {
        public override void Affect() {
            base.Affect();

            // GameManager.instance.ChunkManager.StartFlying();
            // PlayerCore.instance.ChangeState(PlayerState.PrepareFlying);
        }

        public override void Expire() {
            // GameManager.instance.ChunkManager.StopFlying();
            // PlayerCore.instance.ChangeState(PlayerState.Running);

            base.Expire();
        }
    }    
}
