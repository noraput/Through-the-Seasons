using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Umbrella : TemporaryItem {
        public Umbrella() {
            itemType = ItemType.Umbrella;
        }

        public override void Affect() {
            base.Affect();
        }

        public override void Expire() {
            base.Expire(); 
        }
    }
}
