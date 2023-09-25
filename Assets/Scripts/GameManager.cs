using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // PlayerPrefs
    [HideInInspector] public string controlStateKey = "ControlScheme";

    [Header("Player")]
    public Transform playerSpawn;
    public GameObject playerPrefab;
    [HideInInspector] public Transform player;
    [HideInInspector] public PlayerMovement playerMovementScript;

    [Header("Level")]
    public GameObject[] levelPieces;
    private Queue<GameObject> spawnedLevelPieces;
    public float groundHeightOfLevel = -1f;
    private Vector3 levelSpawnPos;

    [Header("Level Settings")]
    public bool isSpawningLevel = true;
    public bool isBossBattle = false;
    public bool respawnPlayerAtCheckpoints = true;
    public bool isCheckingForFailure = false;
    [HideInInspector] public bool playerFailed;

    [Header("Boss Battle")]
    public GameObject bossPrefab;
    public GameObject[] bossPlatforms;
    public GameObject bossInstance;

    // This tells us which piece should be spawned
    private int currentPiece = 0;
    private int currentCheckpoint;

    // These are used to keep track of when a new piece should be spawned
    private float positionOnCurrentPiece;
    private float lengthOfPiece = 30f; // All of the pieces have been designed to have an x-value of 30

    public static GameManager instance;
    LevelProgress LevelProgress;

    private float gameSpeed;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameSpeed = 1f;

        // Instantiate queue
        spawnedLevelPieces = new Queue<GameObject>();

        if (bossPrefab != null)
        {
            isBossBattle = true;
        }

        currentPiece = 0;
        currentCheckpoint = 0;

        // Set-up level
        if (isSpawningLevel)
        {
            BeginLevel();
        }

        SpawnPlayer();
    }

    void Update()
    {
        if (playerMovementScript == null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }

        if (player.position.x > 0 &&
            player.position.x % lengthOfPiece < positionOnCurrentPiece)
        {
            if (isSpawningLevel)
            {
                SpawnNextPiece();
            }
            else if (isBossBattle)
            {
                SpawnBossLevelPiece();
                bossInstance.GetComponent<Boss>().NextPhase();
            }
        }

        positionOnCurrentPiece = player.position.x % lengthOfPiece;
    }

    public void ChangeDifficulty(string difficultyType)
    {
        switch (difficultyType)
        {
            case "easy":
                gameSpeed = 0.7f;
                LevelProgress.speed = false;
                LevelProgress.setSpeedText();
                break;
            case "hard":
                gameSpeed = 1.0f;
                LevelProgress.speed = true;
                LevelProgress.setSpeedText();
                break;
        }
    }

    public void BeginLevel()
    {
        // Clear queue
        int numOfPieces = spawnedLevelPieces.Count;
        for (int i = 0; i < numOfPieces; i++)
        {
            Destroy(spawnedLevelPieces.Dequeue());
        }

        // Spawn two starting pieces
        levelSpawnPos = new Vector3(0f, groundHeightOfLevel, 0f);
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[currentPiece], levelSpawnPos, Quaternion.identity));

        levelSpawnPos.x = lengthOfPiece;
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[currentPiece + 1], levelSpawnPos, Quaternion.identity));

        currentPiece = currentPiece + 2;
    }

    void SpawnNextPiece()
    {
        // Find out where to spawn new piece
        int distanceTravelled = Mathf.FloorToInt(player.position.x / lengthOfPiece) + 1;
        levelSpawnPos.x = distanceTravelled * lengthOfPiece;

        // Spawn new piece and destroy piece off screen
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[currentPiece], levelSpawnPos, Quaternion.identity));
        Destroy(spawnedLevelPieces.Dequeue());

        // Increment through level pieces
        currentPiece++;
        if (currentPiece == levelPieces.Length)
        {
            if (!isBossBattle)
            {
                // Wrap around to zero if the end of the array has been reached
                currentPiece = 0;
            }
            else
            {
                // Begin boss battle phase
                isSpawningLevel = false;
                CreateBoss();
            }
        }

        // Checkpoints
        switch (currentPiece)
        {
            case 4:
                currentCheckpoint = 4;
                break;
            case 9:
                currentCheckpoint = 9;
                break;
        }
    }

    public void SpawnPlayer()
    {
        // Respawn player
        if (player != null)
        {
            Debug.Log("Respawning at checkpoint " + currentCheckpoint);
            player.position = playerSpawn.position;
            return;
        }
        else
        {
            GameObject p = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
            player = p.transform;
            playerMovementScript = player.GetComponent<PlayerMovement>();
        }

        playerMovementScript.isGrounded = false;
    }

    public void PlayerIsHit()
    {
        if (respawnPlayerAtCheckpoints)
        {
            // Update scores
            LevelProgress.instance.deaths = LevelProgress.instance.deaths + 1;
            LevelProgress.instance.setDeathText();

            // Reset
            if (currentPiece < 4)
            {
                currentCheckpoint = 0;
            } else if (currentPiece < 9)
            {
                currentCheckpoint = 4;
            } else
            {
                currentCheckpoint = 9;
            }

            currentPiece = currentCheckpoint;
            SpawnPlayer();
            BeginLevel();
        }

        if (isCheckingForFailure)
        {
            playerFailed = true;
        }
    }

    void SpawnBossLevelPiece()
    {
        // Find out where to spawn new piece
        int distanceTravelled = Mathf.FloorToInt(player.position.x / lengthOfPiece) + 1;
        levelSpawnPos.x = distanceTravelled * lengthOfPiece;

        // Choose a random platform to spawn
        int randomPiece = Random.Range(0, bossPlatforms.Length - 1);

        // Spawn new piece and destroy piece off screen
        spawnedLevelPieces.Enqueue(Instantiate(bossPlatforms[randomPiece], levelSpawnPos, Quaternion.identity));
        Destroy(spawnedLevelPieces.Dequeue());
    }

    void CreateBoss()
    {
        // Find out where to spawn new piece
        int distanceTravelled = Mathf.FloorToInt(player.position.x / lengthOfPiece) + 1;
        levelSpawnPos.x = distanceTravelled * lengthOfPiece;

        bossInstance = Instantiate(bossPrefab, levelSpawnPos, Quaternion.identity);
    }

    public void EndLevel()
    {
        // Add code to change scene to the level select screen
        SceneManager.LoadScene("EndScreen");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = gameSpeed;
    }
}
