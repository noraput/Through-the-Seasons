using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThroughTheSeasons
{
    public class GameManager : PersistentObject<GameManager>
    {
        private Transform playerTransform;
        public Transform PlayerTransform { get => playerTransform; }

        private Vector2 playerStartingPosition;
        public Vector2 PlayerStartingPosition { get => playerStartingPosition; }

        private Vector2 lastSeasonPosition;
        private SpawnedChunk currentChunk;
        public SpawnedChunk CurrentChunk { get => currentChunk; }

        private ChunkManager chunkManager;
        public ChunkManager ChunkManager { get => chunkManager; }

        public int CurrentTileOrder { get; set; }
        public TileCreateState TileState { get; set; }

        #region GameplayData
        [SerializeField]
        private float fallLimit = 20f; 
        
        private float xDistance;
        public float XDistance { get => xDistance; }
        
        private float distanceInThisSeason;

        [SerializeField]
        private int startingRequiredChunks;
        
        [SerializeField]
        private int requiredChunksIncreaseRate;
        private int requiredChunksToNextSeason;
        private int chunksPassedInThisSeason;
        private int chunksPassed;

        [SerializeField]
        private int seasonsPerYear;
        private int seasonsPassed;
        private int yearPassed;

        [SerializeField]
        private Season[] seasons = new Season[] {
            Season.Summer,
            Season.Spring,
            Season.Autumn,
            Season.Winter 
        };

        [SerializeField]
        private int startingSeasonIndex;
        private int currentSeasonIndex;
        
        private Season currentSeason;
        public Season CurrentSeason { get => currentSeason; }
        #endregion
        
        public event Action<Season> OnSeasonChange;
        public event Action<int> OnLifeChanged;

        public int score;
        public int life = 4;
        public int coin;
        public int bonusScore;
        
        protected override void Awake() {
            base.Awake();

            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            playerStartingPosition = playerTransform.position;

            Initialize();
        }

        private void Initialize() {
            chunkManager = GetComponent<ChunkManager>();
            currentChunk = GameObject.FindGameObjectWithTag("StartingChunk").GetComponent<SpawnedChunk>();
            requiredChunksToNextSeason = startingRequiredChunks;
            
            currentSeasonIndex = startingSeasonIndex;
            currentSeason = GetCurrentSeason();
            lastSeasonPosition = Vector2.zero;
            
            OnLifeChanged?.Invoke(life);
        }

        private void OnEnable() {
            SpawnedChunk.OnChunkEnter += UpdateCurrentChunk;
            SpawnedChunk.OnChunkExit += UpdateChunkInfo;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SpawnedChunk.OnChunkExit -= UpdateCurrentChunk;
            SpawnedChunk.OnChunkExit -= UpdateChunkInfo;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            Initialize();
        }

        private void UpdateCurrentChunk(SpawnedChunk chunk) {
            currentChunk = chunk;

            if (!currentChunk.name.Contains("Flying")) {
                PlayerCore.instance.ChangeState(PlayerState.Running);
            }
        }

        private void UpdateChunkInfo(SpawnedChunk chunk) {
            if (chunk.isStartingChunk)
                return;
                
            chunksPassed++;
            chunksPassedInThisSeason++;

            if (chunksPassedInThisSeason >= requiredChunksToNextSeason) {
                chunksPassedInThisSeason = 0;
                requiredChunksToNextSeason += requiredChunksIncreaseRate;

                GoToNextSeason(chunk);
            }
        }

        private void GoToNextSeason(SpawnedChunk chunk) {
            lastSeasonPosition = chunk.GetEndPosition();

            currentSeasonIndex++;
            currentSeason = GetCurrentSeason();

            seasonsPassed++;
            yearPassed = seasonsPassed / seasonsPerYear;

            OnSeasonChange?.Invoke(currentSeason);
        }

        private void Update() {
            xDistance = GetDistanceFromPlayer(playerStartingPosition.x);
            distanceInThisSeason = GetDistanceFromPlayer(lastSeasonPosition.x);
            
            score = ((int) xDistance) + (coin * Coin.score) + bonusScore;
        }

        public float GetDistanceFromPlayer(float target) {
            return Mathf.Abs(playerTransform.position.x - target);
        }

        public float GetFallDistanceFromPlayer(float target) {
            return target - playerTransform.position.y;
        }

        private Season GetCurrentSeason() {
            return seasons[currentSeasonIndex % seasons.Length];
        }

        public Season GetSeason(int seasonIndex) {
            return seasons[seasonIndex % seasons.Length];
        }

        public void UpdateLife(int life) {
            this.life = Mathf.Clamp(this.life + life, 0, 4);
            OnLifeChanged?.Invoke(life);
        }

        // private Season GetSeason(int offset) {
        //     return seasons[(currentSeasonIndex + offset) % seasons.Length];
        // }

        public bool IsPlayerFallFromCurrentChunk() {
            return GetFallDistanceFromPlayer(currentChunk.transform.position.y) >= fallLimit;
        }

        #region Debug
        public string GetDebugInfo() {
            return $"Chunk: {currentChunk.name}"
                + $"\nDistance: {(int) xDistance} | Season Dist: {(int) distanceInThisSeason}"
                + $"\nChunks Passed: {chunksPassed} | Season Chunks: {chunksPassedInThisSeason} | Required Chunks: {requiredChunksToNextSeason}"
                + $"\nSeasons Passed: {seasonsPassed} | Seasons Left: {seasons.Length - (currentSeasonIndex % seasons.Length)} | Years Passed: {yearPassed}"
                + $"\nCurrent Season: {GetCurrentSeason()} | Next Season: {GetSeason(1)}";
        }
        #endregion
    }
}
