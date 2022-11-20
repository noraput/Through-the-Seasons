using System;
using UnityEngine;

namespace ThroughTheSeasons {
    public class CollectibleItem : Collectible {
        public Item holdingItem;
        public ItemType itemType;
        private SpriteRenderer itemSpriteRenderer;
        private float itemScale = 1.5f;

        public static Action<ItemType> OnCollect; 

        public void Initialize(Season season) {
            transform.localScale *= itemScale;
            itemSpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();

            // holdingItem = ItemAssets.instance.GetItem(itemType, season);
            holdingItem = ItemAssets.instance.GetItem(ItemType.HealthPotion, season);
            SetItemSprite();
        }

        private void SetItemSprite() {
            ItemSpriteInfo spriteInfo = ItemAssets.instance.GetItemSprite(holdingItem);
            itemSpriteRenderer.sprite = spriteInfo.sprite;
            itemSpriteRenderer.transform.localPosition = spriteInfo.offset;
            itemSpriteRenderer.transform.localScale = spriteInfo.scale;

            Vector3 rotation = spriteInfo.rotation;
            itemSpriteRenderer.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }

        public override void Collect() {
            holdingItem?.Apply();
            OnCollect?.Invoke(holdingItem.itemType);
            
            // if (itemType != ItemType.RandomInCurrentSeason) {
            //     Debug.Log("This " + holdingItem.GetType().Name + " is fixed in the level");
            // }

            base.Collect();
        }
    }
}
