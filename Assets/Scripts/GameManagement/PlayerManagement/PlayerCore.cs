using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

namespace ThroughTheSeasons
{
    public class PlayerCore : PersistentObject<PlayerCore>
    {
        private BoxCollider2D col;
        public BoxCollider2D Col { get => col; }

        private CharacterBase charBase;
        public CharacterBase CharBase { get => charBase; }

        public List<TemporaryItem> usingItems;
        public List<TemporaryItem> expiredItems;

        public PlayerState state;
        private CinemachineFramingTransposer composer;

        [SerializeField]
        Animator anim;

        [SerializeField]
        CinemachineVirtualCamera playerCam;

         private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Initialize();
        }

        private void Initialize() {
            charBase.Movement.Reset();
            usingItems.Clear();
            expiredItems.Clear();
        } 

        protected override void Awake() {
            base.Awake();

            col = GetComponent<BoxCollider2D>();
            charBase = GetComponent<CharacterBase>();

            usingItems = new List<TemporaryItem>();
            expiredItems = new List<TemporaryItem>();

            composer = playerCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update() {
            //Debug.Log(usingItems.Count + " | " + expiredItems.Count);
            
            HandleItems();
            CheckIfPlayerIsFalling();
        }

        public void CheckItem(TemporaryItem item) {
            if (AlreadyHasItem(item)) {
                GetDuplicatedItem(item).Reset();
                return;
            }

            usingItems.Add(item);
            item.Affect();
        }

        public bool AlreadyHasItem(TemporaryItem item) {
            return usingItems.Any(i => i.GetType() == item.GetType());
        }

        public bool HasItemOfType(ItemType itemType) {
            return usingItems.Any(i => i.GetType() == ItemAssets.instance.GetItem(itemType).GetType());
        }

        public TemporaryItem GetDuplicatedItem(TemporaryItem item) {
            return usingItems.First(i => i.GetType() == item.GetType());
        }

        private void HandleItems() {
            if (expiredItems.Count > 0) {
                foreach (TemporaryItem item in expiredItems) {
                    usingItems.Remove(item);
                }

                expiredItems.Clear();
            }

            if (usingItems.Count > 0) {
                foreach (TemporaryItem item in usingItems) {
                    item.TryTick();

                    if (item.isExpired) {
                        expiredItems.Add(item);
                    }
                }
            }
        }

        private void CheckIfPlayerIsFalling() {
            if (GameManager.instance.IsPlayerFallFromCurrentChunk() && CompareState(PlayerState.Running)) {
                SceneManager.LoadScene(0);
                transform.position = GameManager.instance.PlayerStartingPosition;
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.CompareTag("Obstacle")) {
                if (HasItemOfType(ItemType.BigPotion) || HasItemOfType(ItemType.SpeedShoes)) {
                    if (col.gameObject && col.gameObject) {
                        Destroy(col.gameObject);
                        GameManager.instance.bonusScore += 1000;
                    }
                }
                else {
                    anim.SetTrigger("Hit");
                    GameManager.instance.UpdateLife(-1);
                }

                if (GameManager.instance.life <= 0)
                {
                    // Debug.Log("You ran out of Life");
                }
            }

            // else if (col.CompareTag("Deadzone")) {
            //     SceneManager.LoadScene(0);
            //     transform.position = GameManager.instance.playerStartingPosition;
            // }
        }

        public void ChangeState(PlayerState state) {
            this.state = state;

            if (state == PlayerState.PrepareFlying) {
                // composer.m_DeadZoneHeight = 0f;

                GetComponent<BetterJumping>().enabled = false;
            }
            else if (state == PlayerState.Flying) {
                // composer.m_DeadZoneHeight = 2f;

                SpawnedChunk[] chunks = FindObjectsOfType<SpawnedChunk>();
                foreach (SpawnedChunk chunk in chunks)
                {
                    if (!chunk.name.Contains("Flying")) {
                        Destroy(chunk.gameObject);
                    }
                }
            }
            else if (state == PlayerState.Running) {
                GetComponent<BetterJumping>().enabled = true;

                // composer.m_DeadZoneHeight = 0f;
            }
        }

        public bool CompareState(PlayerState state) {
            return this.state == state;
        }
    }
}
