using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    private float lengthOfPiece = 20f; // All of the pieces have been designed to have an x-value of 20

    public static GameManager instance;
    LevelProgress LevelProgress;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Instantiate queue
        spawnedLevelPieces = new Queue<GameObject>();

        if (bossPrefab != null )
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
        switch(difficultyType)
        {
            case "easy":
<<<<<<< HEAD
                playerMovementScript.ChangePlayerSpeed(5.0f);
                break;
            case "hard":
                playerMovementScript.ChangePlayerSpeed(8.0f);
=======
                playerMovementScript.ChangePlayerSpeed(0.5f);
                LevelProgress.speed = false;
                LevelProgress.setSpeedText();
                break;
            case "hard":
                playerMovementScript.ChangePlayerSpeed(1f); 
                LevelProgress.speed = true;
                LevelProgress.setSpeedText();
>>>>>>> UI
                break;
        }
    }

    public void BeginLevel()
    {
        Time.timeScale = 1.0f;

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
            } else
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
            player.position = playerSpawn.position;
            return;
        }

        GameObject p = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        player = p.transform;
        playerMovementScript = player.GetComponent<PlayerMovement>();
    }

    // This connects the UI buttons to the player script
    public void ButtonJump()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonJump();
    }

    public void ButtonSlide()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonSlide();
    }

    public void ButtonAttack()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonAttack();
    }

    public void PlayerIsHit()
    {
        if (respawnPlayerAtCheckpoints)
        {
            currentPiece = currentCheckpoint;
            SpawnPlayer();
            LevelProgress.deaths = LevelProgress.deaths + 1;
            LevelProgress.setDeathText();
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
        Time.timeScale = 1.0f;
    }

    public void SwipeControls(bool canSwipe)
    {
        playerMovementScript.isSwipingEnabled = canSwipe;
    }
}
