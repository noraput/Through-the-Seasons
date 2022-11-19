using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons
{
    public class Magnet : TemporaryItem {
        public Magnet() {
            itemType = ItemType.Magnet;
        }

        public override void Affect() {
            base.Affect();
        }

        public override void Expire() {
            base.Expire(); 
        }
    }    
}
