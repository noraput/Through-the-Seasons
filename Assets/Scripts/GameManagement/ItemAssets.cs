using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThroughTheSeasons
{
    public class SeasonalPool<T> {
        public Season season;
        public List<T> pool;
    }

    [Serializable]
    public class ItemPool : SeasonalPool<ItemType> {}

    public class ItemAssets : PersistentObject<ItemAssets>
    {
        public List<ItemPool> itemPools;

        public Item GetItem(ItemType itemType, Season season = Season.Default) {
            switch (itemType) {
                case ItemType.RandomInCurrentSeason: {
                    return GetItem(PickRandomItemFromPool(GetPoolFromSeason(season)));
                }
                case ItemType.BigPotion: {
                    return new BigPotion(3f);
                }
                case ItemType.Magnet: {
                    return new Magnet();
                }
                case ItemType.Rocket: {
                    return new Rocket();
                }
                case ItemType.HealthPotion: {
                    return new HealthPotion(1);
                }
                case ItemType.SpeedShoes: {
                    return new SpeedShoes(2f);
                }
                case ItemType.Sycthe: {
                    return new Sycthe();
                }
                case ItemType.Umbrella: {
                    return new Umbrella();
                }
                case ItemType.IceSkateShoes: {
                    return new IceSkateShoes();
                }
                case ItemType.BigCoin: {
                    return new BigCoin(100f, 10);
                }
                case ItemType.None: {
                    Debug.Log("There is no Item in " + season);
                    return null;
                }
                default: {
                    Debug.LogError("No matching item for this Item Type!");
                    return null;
                }
            }
        }

        public List<ItemType> GetPoolFromSeason(Season season, bool includesGeneralItems = true) {
            List<ItemType> itemPoolInThisSeason = itemPools.First(
                pool => pool.season == season
            ).pool;
            
            return (includesGeneralItems
                ? itemPoolInThisSeason.Union(GetGeneralItemPool())
                : itemPoolInThisSeason
            ).ToList();
        }

        public List<ItemType> GetGeneralItemPool() {
            return itemPools.First(pool => pool.season == Season.Any).pool.ToList();
        }

        public ItemType PickRandomItemFromPool(List<ItemType> pool) {
            if (!pool.Any()) {
                return ItemType.None;
            }

            return pool.PickRandom();
        }
    }
}
