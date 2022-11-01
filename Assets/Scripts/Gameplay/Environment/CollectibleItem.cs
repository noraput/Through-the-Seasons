using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheSeasons {
    public class CollectibleItem : Collectible {
        public Item holdingItem;
        public ItemType itemType;

        public void Initialize(Season season) {
            holdingItem = ItemAssets.instance.GetItem(itemType, season);
        }

        public override void Collect() {
            holdingItem?.Affect();
            
            if (itemType != ItemType.RandomInCurrentSeason) {
                Debug.Log("This " + holdingItem.GetType().Name + " is fixed in the level");
            }

            base.Collect();
        }
    }
}
